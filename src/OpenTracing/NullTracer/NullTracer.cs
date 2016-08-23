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

        public void Inject(ISpanContext spanContext, string format, IInjectCarrier carrier)
        {
        }

        public ISpanContext Extract(string format, IExtractCarrier carrier)
        {
            return null;
        }
    }
}