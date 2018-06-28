using System;
using System.Collections.Generic;
using System.Text;

namespace OpenTracing.Decorators
{
    sealed class ScopeManagerFactoryDecorator : IScopeManager
    {
        private readonly IScopeManager _scopeManager;
        private readonly ScopeDecoratorFactory _scopeDecoratorFactory;

        public ScopeManagerFactoryDecorator(IScopeManager scopeManager, ScopeDecoratorFactory scopeDecoratorFactory)
        {
            _scopeManager = scopeManager;
            _scopeDecoratorFactory = scopeDecoratorFactory ?? throw new ArgumentNullException(nameof(scopeDecoratorFactory));
        }

        public IScope Active => _scopeDecoratorFactory(_scopeManager.Active);

        public IScope Activate(ISpan span, bool finishSpanOnDispose) => _scopeDecoratorFactory(_scopeManager.Activate(span, finishSpanOnDispose));
    }
}
