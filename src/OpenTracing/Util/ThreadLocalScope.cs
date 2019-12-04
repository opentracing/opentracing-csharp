namespace OpenTracing.Util
{
    /// <inheritdoc />
    /// <summary>
    /// A <see cref="T:OpenTracing.IScope" /> primitive that relies on thread local storage for
    /// managing active scopes. Intended to be used in systems where multiple logical,
    /// related operations are all executed in the same thread albeit at different times.
    /// </summary>
    public class ThreadLocalScope : IScope
    {
        private readonly ThreadLocalScopeManager _scopeManager;
        private readonly bool _finishOnDispose;
        private readonly IScope _scopeToRestore;

        public ThreadLocalScope(ThreadLocalScopeManager scopeManager, ISpan wrappedSpan, bool finishOnDispose)
        {
            _scopeManager = scopeManager;
            Span = wrappedSpan;
            _finishOnDispose = finishOnDispose;
            _scopeToRestore = scopeManager.Active;
            scopeManager.Active = this;
        }

        public void Dispose()
        {
            if (_scopeManager.Active != this)
            {
                // This shouldn't happen if users call methods in the expected order. Bail out.
                return;
            }

            if (_finishOnDispose)
            {
                Span.Finish();
            }

            _scopeManager.Active = _scopeToRestore;
        }

        public ISpan Span { get; }
    }
}
