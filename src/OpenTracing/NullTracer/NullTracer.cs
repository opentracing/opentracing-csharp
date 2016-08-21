using OpenTracing.Propagation;

namespace OpenTracing.NullTracer
{
    public class NullTracer : ITracer
    {
        public static readonly NullTracer Instance = new NullTracer();

        private NullTracer()
        {
        }

        public ISpan StartSpan(string operationName, StartSpanOptions options = null)
        {
            return NullSpan.Instance;
        }

        public void Inject(ISpanContext spanContext, IInjectCarrier carrier)
        {
        }

        public ISpanContext Extract(IExtractCarrier carrier)
        {
            return null;
        }
    }
}