using OpenTracing.Propagation;

namespace OpenTracing.Noop
{
    public sealed class NoopTracer : ITracer
    {
        internal static readonly NoopTracer Instance = new NoopTracer();

        public IScopeManager ScopeManager => NoopScopeManager.Instance;

        public ISpan ActiveSpan => null;

        private NoopTracer()
        {
        }

        public ISpanBuilder BuildSpan(string operationName)
        {
            return NoopSpanBuilder.Instance;
        }

        public void Inject<TCarrier>(ISpanContext spanContext, IFormat<TCarrier> format, TCarrier carrier)
        {
        }

        public ISpanContext Extract<TCarrier>(IFormat<TCarrier> format, TCarrier carrier)
        {
            return NoopSpanContext.Instance;
        }

        public override string ToString()
        {
            return nameof(NoopTracer);
        }
    }
}
