using System;
using OpenTracing.Propagation;

namespace OpenTracing.NullTracer
{
    public class NullTracer : ITracer
    {
        public ISpan StartSpan(string operationName)
        {
            return new NullSpan(this);
        }

        public ISpan StartSpan(string operationName, DateTimeOffset startTimestamp)
        {
            return new NullSpan(this);
        }

        public void Inject(ISpanContext spanContext, IInjectCarrier carrier)
        {
        }

        public ISpanContext Extract(IExtractCarrier carrier)
        {
            return new NullSpanContext();
        }
    }
}