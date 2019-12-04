using System;

namespace OpenTracing.Util
{
    /// <summary>
    /// An <see cref="IScopeManager"/> implementation that relies on thread local storage for
    /// managing active scopes. Intended to be used in systems where multiple logical,
    /// related operations are all executed in the same thread albeit at different times.
    /// </summary>
    public class ThreadLocalScopeManager : IScopeManager
    {
        /*
         * Went with ThreadStatic over ThreadLocal<T> because we don't
         * want scopes to be initialized upon thread initialization by default
         * and we want ThreadStatic's mutability for restoring / replacing scopes.
         */
        [ThreadStatic]
        private static IScope _active;

        public IScope Active
        {
            get => _active;
            set => _active = value;
        }
        public IScope Activate(ISpan span, bool finishSpanOnDispose)
        {
            return new ThreadLocalScope(this, span, finishSpanOnDispose);
        }
    }
}
