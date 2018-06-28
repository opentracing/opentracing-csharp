using System;
using System.Collections.Generic;
using System.Text;

namespace OpenTracing.Decorators
{
    public class ScopeManagerDecorator : IScopeManager
    {
        private readonly IScopeManager _scopeManager;
        private readonly ScopeDecoratorFactory _scopeDecoratorFactory;

        public ScopeManagerDecorator(IScopeManager scopeManager, ScopeDecoratorFactory scopeDecoratorFactory = null)
        {
            _scopeManager = scopeManager;
            _scopeDecoratorFactory = scopeDecoratorFactory ?? DefaultDecoratorFactories.DefaultScopeDecoratorFactory;
        }

        public virtual IScope Active => _scopeDecoratorFactory(_scopeManager.Active);

        public virtual IScope Activate(ISpan span, bool finishSpanOnDispose) => _scopeDecoratorFactory(_scopeManager.Activate(span, finishSpanOnDispose));
    }
}
