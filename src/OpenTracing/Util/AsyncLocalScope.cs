namespace OpenTracing.Util
{
    /// <summary>
    /// The <see cref="AsyncLocalScope"/> is a simple <see cref="IScope"/> implementation
    /// that relies on C#'s AsyncLocal/CallContext storage primitive.
    /// </summary>
    /// <seealso cref="IScopeManager"/>
    public class AsyncLocalScope : IScope
    {
        private readonly AsyncLocalScopeManager _scopeManager;
        private readonly ISpan _wrappedSpan;
        private readonly bool _finishOnDispose;
        private readonly IScope _scopeToRestore;

        public AsyncLocalScope(AsyncLocalScopeManager scopeManager, ISpan wrappedSpan, bool finishOnDispose)
        {
            _scopeManager = scopeManager;
            _wrappedSpan = wrappedSpan;
            _finishOnDispose = finishOnDispose;

            _scopeToRestore = scopeManager.Active;
            scopeManager.Active = this;
        }

        public ISpan Span => _wrappedSpan;

        public void Dispose()
        {
            if (_scopeManager.Active != this)
            {
                // This shouldn't happen if users call methods in the expected order. Bail out.
                return;
            }

            if (_finishOnDispose)
            {
                _wrappedSpan.Finish();
            }

            _scopeManager.Active = _scopeToRestore;
        }
    }
}
