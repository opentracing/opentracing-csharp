using System;
using System.Collections.Generic;
using System.Text;

namespace OpenTracing.Decorators
{
#pragma warning disable S3881 // "IDisposable" should be implemented correctly => the decorator does not have own resources to dispose
    public class ScopeDecorator : IScope
#pragma warning restore S3881 // "IDisposable" should be implemented correctly
    {
        private readonly IScope _scope;
        private readonly SpanDecoratorFactory _spanDecoratorFactory;

        public ScopeDecorator(IScope scope, SpanDecoratorFactory spanDecoratorFactory)
        {
            _scope = scope;
            _spanDecoratorFactory = spanDecoratorFactory;
        }

        public virtual ISpan Span => _spanDecoratorFactory(_scope.Span);

        public virtual void Dispose() => _scope.Dispose();
    }
}
