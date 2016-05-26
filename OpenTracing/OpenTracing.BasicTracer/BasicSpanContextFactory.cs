using OpenTracing.OpenTracing.Context;
using System.Collections.Generic;
using System.Linq;

namespace OpenTracing.BasicTracer
{
    public class BasicSpanContextFactory : ISpanContextFactory<BasicSpanContext>
    {
        private ulong _sampleRate;
        private const int sampleAll = 1;

        public BasicSpanContextFactory()
            : this (sampleAll)
        {
        }

        public BasicSpanContextFactory(ulong sampleRate)
        {
            _sampleRate = sampleRate;
        }

        private bool ShouldSample(ulong spanId)
        {
            return spanId % _sampleRate == 0;
        }

        public BasicSpanContext NewRootSpanContext()
        {
            var traceId = GuidFactory.Create();
            var spanId = GuidFactory.Create();
            var shouldSample = ShouldSample(spanId);

            var baggage = new Dictionary<string, string>() { };
            return new BasicSpanContext(traceId, 0, spanId, shouldSample, baggage);
        }

        public BasicSpanContext NewChildSpanContext(BasicSpanContext spanContext)
        {
            var parentTraceContext = spanContext;

            var traceId = parentTraceContext.TraceId;
            var parentId = parentTraceContext.SpanId;
            var spanId = GuidFactory.Create();
            var shouldSample = ShouldSample(spanId);

            var baggage = parentTraceContext.GetBaggageItems().ToDictionary(p => p.Key, p => p.Value);

            var childSpanContext = new BasicSpanContext(traceId, parentId, spanId, shouldSample, baggage);
            return childSpanContext;
        }
    }
}
