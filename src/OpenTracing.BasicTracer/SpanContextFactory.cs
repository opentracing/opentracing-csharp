using System;
using System.Linq;

namespace OpenTracing.BasicTracer
{
    public class SpanContextFactory : ISpanContextFactory
    {
        private const short TraceEverything = 100;

        private readonly short _samplingRate;
        private readonly Random _samplingRandom = new Random();

        public SpanContextFactory()
            : this (TraceEverything)
        {
        }

        public SpanContextFactory(short sampleRate)
        {
            if (sampleRate < 0 || sampleRate > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(sampleRate), sampleRate, "Value must be between 0 and 100");
            }

            _samplingRate = sampleRate;
        }

        private bool ShouldSample()
        {
            if (_samplingRate == 100)
            {
                return true;
            }
            
            var random = _samplingRandom.Next(0, 100);
            return random % _samplingRate == 0;
        }

        public SpanContext NewRootSpanContext()
        {
            var traceId = Guid.NewGuid();
            var spanId = Guid.NewGuid();
            var shouldSample = ShouldSample();

            return new SpanContext(traceId, null, spanId, shouldSample);
        }

        public SpanContext NewChildSpanContext(SpanContext parent)
        {
            if (parent == null)
            {
                throw new ArgumentNullException(nameof(parent));
            }

            var traceId = parent.TraceId;
            var parentId = parent.SpanId;
            var spanId = Guid.NewGuid();
            var shouldSample = ShouldSample();

            var baggage = parent.GetBaggageItems().ToDictionary(p => p.Key, p => p.Value);

            return new SpanContext(traceId, parentId, spanId, shouldSample, baggage);
        }
    }
}
