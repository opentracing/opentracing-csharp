using System;
using System.Collections.Generic;
using System.Text;
using OpenTracing.Propagation;

namespace OpenTracing.Decorators
{
    sealed class TracerFactoryDecorator : ITracer
    {
        private readonly ITracer _tracer;
        private readonly ScopeManagerDecoratorFactory _scopeManagerDecoratorFactory;
        private readonly SpanDecoratorFactory _spanDecoratorFactory;
        private readonly SpanBuilderDecoratorFactory _spanBuilderDecoratorFactory;
        private readonly SpanContextDecoratorFactory _spanContextDecoratorFactory;

        public TracerFactoryDecorator(
            ITracer tracer,
            ScopeManagerDecoratorFactory scopeManagerDecoratorFactory,
            SpanDecoratorFactory  spanDecoratorFactory,
            SpanBuilderDecoratorFactory spanBuilderDecoratorFactory,
            SpanContextDecoratorFactory spanContextDecoratorFactory
            )
        {
            _tracer = tracer;
            _scopeManagerDecoratorFactory = scopeManagerDecoratorFactory ?? throw new ArgumentNullException(nameof(scopeManagerDecoratorFactory));
            _spanDecoratorFactory = spanDecoratorFactory ?? throw new ArgumentNullException(nameof(spanDecoratorFactory));
            _spanBuilderDecoratorFactory = spanBuilderDecoratorFactory ?? throw new ArgumentNullException(nameof(spanBuilderDecoratorFactory));
            _spanContextDecoratorFactory = spanContextDecoratorFactory ?? throw new ArgumentNullException(nameof(spanContextDecoratorFactory));
        }

        public IScopeManager ScopeManager => _scopeManagerDecoratorFactory(_tracer.ScopeManager);

        public ISpan ActiveSpan => _spanDecoratorFactory(_tracer.ActiveSpan);

        public ISpanBuilder BuildSpan(string operationName) => _spanBuilderDecoratorFactory(_tracer.BuildSpan(operationName));

        public ISpanContext Extract<TCarrier>(IFormat<TCarrier> format, TCarrier carrier) => _spanContextDecoratorFactory(_tracer.Extract(format, carrier));

        public void Inject<TCarrier>(ISpanContext spanContext, IFormat<TCarrier> format, TCarrier carrier) => _tracer.Inject(spanContext, format, carrier);
    }
}
