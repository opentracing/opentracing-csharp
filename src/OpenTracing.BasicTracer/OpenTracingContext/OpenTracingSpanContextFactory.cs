using OpenTracing.BasicTracer.Context;
using System.Collections.Generic;
using System.Linq;

namespace OpenTracing.BasicTracer.OpenTracingContext
{
    public class OpenTracingSpanContextFactory : ISpanContextFactory<OpenTracingSpanContext>
    {
        private ulong _sampleRate;
        private const int sampleAll = 1;

        public OpenTracingSpanContextFactory()
            : this (sampleAll)
        {
        }

        public OpenTracingSpanContextFactory(ulong sampleRate)
        {
            _sampleRate = sampleRate;
        }

        private bool ShouldSample(ulong spanId)
        {
            return spanId % _sampleRate == 0;
        }

        public OpenTracingSpanContext NewRootSpanContext()
        {
            var traceId = GuidFactory.Create();
            var spanId = GuidFactory.Create();
            var shouldSample = ShouldSample(spanId);

            var baggage = new Dictionary<string, string>() { };
            return new OpenTracingSpanContext(traceId, 0, spanId, shouldSample, baggage);
        }

        public OpenTracingSpanContext NewChildSpanContext(OpenTracingSpanContext spanContext)
        {
            var parentTraceContext = spanContext;

            var traceId = parentTraceContext.TraceId;
            var parentId = parentTraceContext.SpanId;
            var spanId = GuidFactory.Create();
            var shouldSample = ShouldSample(spanId);

            var baggage = parentTraceContext.GetBaggageItems().ToDictionary(p => p.Key, p => p.Value);

            var childSpanContext = new OpenTracingSpanContext(traceId, parentId, spanId, shouldSample, baggage);
            return childSpanContext;
        }
    }
}
