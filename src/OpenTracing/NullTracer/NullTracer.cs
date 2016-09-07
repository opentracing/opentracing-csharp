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

        public ISpanBuilder BuildSpan(string operationName)
        {
            return NullSpanBuilder.Instance;
        }

        public void Inject<TFormat>(ISpanContext spanContext, IInjectCarrier<TFormat> carrier)
        {
        }

        public ISpanContext Extract<TFormat>(IExtractCarrier<TFormat> carrier)
        {
            return null;
        }
    }
}