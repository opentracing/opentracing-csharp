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

        public ScopeDecorator(IScope scope)
        {
            _scope = scope;
        }

        public virtual ISpan Span => _scope.Span;

        public virtual void Dispose() => _scope.Dispose();
    }
}
