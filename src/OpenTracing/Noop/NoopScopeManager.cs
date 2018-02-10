namespace OpenTracing.Noop
{
    /// <summary>
    /// A noop (i.e., cheap-as-possible) implementation of an <see cref="IScopeManager"/>.
    /// </summary>
    internal sealed class NoopScopeManager : IScopeManager
    {
        internal static readonly NoopScopeManager Instance = new NoopScopeManager();

        public IScope Active => null;

        private NoopScopeManager()
        {
        }

        public IScope Activate(ISpan span, bool finishSpanOnClose)
        {
            return NoopScope.Instance;
        }

        public override string ToString()
        {
            return nameof(NoopScopeManager);
        }

        public sealed class NoopScope : IScope
        {
            internal static readonly NoopScope Instance = new NoopScope();

            public ISpan Span => NoopSpan.Instance;

            private NoopScope()
            {
            }

            public void Dispose()
            {
            }

            public override string ToString()
            {
                return nameof(NoopScope);
            }
        }
    }
}
