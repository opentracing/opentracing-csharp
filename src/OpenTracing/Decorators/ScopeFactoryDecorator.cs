using System;
using System.Collections.Generic;
using System.Text;

namespace OpenTracing.Decorators
{
#pragma warning disable S3881 // "IDisposable" should be implemented correctly => the decorator does not have own resources to dispose
    sealed class ScopeFactoryDecorator : IScope
#pragma warning restore S3881 // "IDisposable" should be implemented correctly
    {
        private readonly IScope _scope;
        private readonly SpanDecoratorFactory _spanDecoratorFactory;

        public ScopeFactoryDecorator(IScope scope, SpanDecoratorFactory spanDecoratorFactory)
        {
            _scope = scope;
            _spanDecoratorFactory = spanDecoratorFactory ?? throw new ArgumentNullException(nameof(spanDecoratorFactory));
        }

        public ISpan Span => _spanDecoratorFactory(_scope.Span);

        public void Dispose() => _scope.Dispose();
    }
}
