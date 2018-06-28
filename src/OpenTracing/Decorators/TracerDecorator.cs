using System;
using System.Collections.Generic;
using System.Text;
using OpenTracing.Propagation;

namespace OpenTracing.Decorators
{
    public class TracerDecorator : ITracer
    {
        private readonly ITracer _tracer;
        private readonly ScopeManagerDecoratorFactory _scopeManagerDecoratorFactory;
        private readonly SpanDecoratorFactory _spanDecoratorFactory;
        private readonly SpanBuilderDecoratorFactory _spanBuilderDecoratorFactory;
        private readonly SpanContextDecoratorFactory _spanContextDecoratorFactory;

        public TracerDecorator(
            ITracer tracer,
            ScopeManagerDecoratorFactory scopeManagerDecoratorFactory = null,
            SpanDecoratorFactory  spanDecoratorFactory = null,
            SpanBuilderDecoratorFactory spanBuilderDecoratorFactory = null,
            SpanContextDecoratorFactory spanContextDecoratorFactory = null
            )
        {
            _tracer = tracer;
            _scopeManagerDecoratorFactory = scopeManagerDecoratorFactory ?? DefaultDecoratorFactories.DefaultScopeManagerDecoratorFactory;
            _spanDecoratorFactory = spanDecoratorFactory ?? DefaultDecoratorFactories.DefaultSpanDecoratorFactory;
            _spanBuilderDecoratorFactory = spanBuilderDecoratorFactory ?? DefaultDecoratorFactories.DefaultSpanBuilderDecoratorFactory;
            _spanContextDecoratorFactory = spanContextDecoratorFactory ?? DefaultDecoratorFactories.DefaultSpanContextDecoratorFactory;
        }

        public virtual IScopeManager ScopeManager => _scopeManagerDecoratorFactory(_tracer.ScopeManager);

        public virtual ISpan ActiveSpan => _spanDecoratorFactory(_tracer.ActiveSpan);

        public virtual ISpanBuilder BuildSpan(string operationName) => _spanBuilderDecoratorFactory(_tracer.BuildSpan(operationName));

        public virtual ISpanContext Extract<TCarrier>(IFormat<TCarrier> format, TCarrier carrier) => _spanContextDecoratorFactory(_tracer.Extract(format, carrier));

        public virtual void Inject<TCarrier>(ISpanContext spanContext, IFormat<TCarrier> format, TCarrier carrier) => _tracer.Inject(spanContext, format, carrier);
    }
}
