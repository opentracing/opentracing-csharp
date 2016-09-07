using System.Collections.Generic;
using System.Linq;

namespace OpenTracing.BasicTracer.Context
{
    public class SpanContextFactory : ISpanContextFactory<SpanContext>
    {
        private ulong _sampleRate;
        private const int sampleAll = 1;

        public SpanContextFactory()
            : this(sampleAll)
        {
        }

        public SpanContextFactory(ulong sampleRate)
        {
            _sampleRate = sampleRate;
        }

        private bool ShouldSample(ulong spanId)
        {
            return spanId % _sampleRate == 0;
        }

        public SpanContext NewRootSpanContext()
        {
            var traceId = GuidFactory.Create();
            var spanId = GuidFactory.Create();
            var shouldSample = ShouldSample(spanId);

            var baggage = new Baggage();
            return new SpanContext(traceId, 0, spanId, shouldSample, baggage);
        }

        public SpanContext NewChildSpanContext(IList<SpanReference> references)
        {
            var reference = references.FirstOrDefault();

            var parentTraceContext = (SpanContext)reference.Context;

            var traceId = parentTraceContext.TraceId;
            var parentId = parentTraceContext.SpanId;
            var spanId = GuidFactory.Create();
            var shouldSample = ShouldSample(spanId);

            var baggage = new Baggage();
            baggage.Merge(parentTraceContext.GetBaggageItems());

            var childSpanContext = new SpanContext(traceId, parentId, spanId, shouldSample, baggage);
            return childSpanContext;
        }
    }
}
