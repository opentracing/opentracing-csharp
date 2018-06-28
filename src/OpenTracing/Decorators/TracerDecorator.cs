using System;
using System.Collections.Generic;
using System.Text;
using OpenTracing.Propagation;

namespace OpenTracing.Decorators
{
    public class TracerDecorator : ITracer
    {
        private readonly ITracer _tracer;

        public TracerDecorator(  ITracer tracer   )
        {
            _tracer = tracer;
        }

        public virtual IScopeManager ScopeManager => _tracer.ScopeManager;

        public virtual ISpan ActiveSpan => _tracer.ActiveSpan;

        public virtual ISpanBuilder BuildSpan(string operationName) => _tracer.BuildSpan(operationName);

        public virtual ISpanContext Extract<TCarrier>(IFormat<TCarrier> format, TCarrier carrier) => _tracer.Extract(format, carrier);

        public virtual void Inject<TCarrier>(ISpanContext spanContext, IFormat<TCarrier> format, TCarrier carrier) => _tracer.Inject(spanContext, format, carrier);
    }
}
