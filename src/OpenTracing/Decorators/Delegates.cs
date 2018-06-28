using System;
using System.Collections.Generic;
using System.Text;

namespace OpenTracing.Decorators
{
    public delegate ITracer TracerDecoratorFactory(ITracer tracer);
    public delegate IScopeManager ScopeManagerDecoratorFactory(IScopeManager scopeManager);
    public delegate ISpan SpanDecoratorFactory(ISpan scopeManager);
    public delegate ISpanBuilder SpanBuilderDecoratorFactory(ISpanBuilder scopeManager);
    public delegate ISpanContext SpanContextDecoratorFactory(ISpanContext scopeManager);
    public delegate IScope ScopeDecoratorFactory(IScope scopeManager);


    public static class DefaultDecoratorFactories
    {
        public static ITracer DefaultTracerDecoratorFactory(ITracer tracer) => tracer;
        public static IScopeManager DefaultScopeManagerDecoratorFactory(IScopeManager scopeManager) => scopeManager;
        public static ISpan DefaultSpanDecoratorFactory(ISpan span) => span;
        public static ISpanBuilder DefaultSpanBuilderDecoratorFactory(ISpanBuilder spanBuilder) => spanBuilder;
        public static ISpanContext DefaultSpanContextDecoratorFactory(ISpanContext spanBuilder) => spanBuilder;
        public static IScope DefaultScopeDecoratorFactory(IScope scope) => scope;
    }
}
