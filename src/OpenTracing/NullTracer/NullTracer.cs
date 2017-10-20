namespace OpenTracing.NullTracer
{
    using OpenTracing.Propagation;

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

        public void Inject<TCarrier>(ISpanContext spanContext, Format<TCarrier> format, TCarrier carrier)
        {
        }

        public ISpanContext Extract<TCarrier>(Format<TCarrier> format, TCarrier carrier)
        {
            return null;
        }
    }
}