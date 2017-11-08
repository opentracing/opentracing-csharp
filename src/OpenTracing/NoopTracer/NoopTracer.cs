using System.Text;

namespace OpenTracing.NoopTracer
{
    using OpenTracing.Propagation;

    public sealed class NoopTracer : ITracer
    {
        public static ITracer Instance = new NoopTracer();

        private NoopTracer()
        {
        }

        public ISpanBuilder BuildSpan(string operationName)
        {
            return NoopSpanBuilder.Instance;
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
