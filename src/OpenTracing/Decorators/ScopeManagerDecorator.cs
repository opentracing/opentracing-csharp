using System;
using System.Collections.Generic;
using System.Text;

namespace OpenTracing.Decorators
{
    public class ScopeManagerDecorator : IScopeManager
    {
        private readonly IScopeManager _scopeManager;

        public ScopeManagerDecorator(IScopeManager scopeManager)
        {
            _scopeManager = scopeManager;
        }

        public virtual IScope Active => _scopeManager.Active;

        public virtual IScope Activate(ISpan span, bool finishSpanOnDispose) => _scopeManager.Activate(span, finishSpanOnDispose);
    }
}
