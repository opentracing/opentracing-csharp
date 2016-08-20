using System;
using OpenTracing.Propagation;

namespace OpenTracing.NullTracer
{
    public class NullTracer : ITracer
    {
        public static readonly NullTracer Instance = new NullTracer();

        private NullTracer()
        {
        }

        public ISpan StartSpan(string operationName)
        {
            return NullSpan.Instance;
        }

        public ISpan StartSpan(string operationName, DateTimeOffset startTimestamp)
        {
            return NullSpan.Instance;
        }

        public void Inject(ISpanContext spanContext, IInjectCarrier carrier)
        {
        }

        public ISpanContext Extract(IExtractCarrier carrier)
        {
            return NullSpanContext.Instance;
        }
    }
}