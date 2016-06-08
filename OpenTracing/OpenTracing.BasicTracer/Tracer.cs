using OpenTracing.BasicTracer.Context;
using OpenTracing.Propagation;
using System;

namespace OpenTracing.BasicTracer
{
    public class Tracer<T> : ITracer where T : ISpanContext
    {
        private readonly ISpanContextFactory<T> _spanContextFactory;
        private ISpanRecorder<T> _spanRecorder;

        internal Tracer(ISpanContextFactory<T> spanContextFactory, ISpanRecorder<T> spanRecorder)
        {
            _spanContextFactory = spanContextFactory;
            _spanRecorder = spanRecorder;
        }

        public void Inject(ISpan span, IInjectCarrier carrier)
        {
            carrier.MapFrom(span);
        }

        public bool TryJoin(string operationName, IExtractCarrier carrier, out ISpan span)
        {
            span = null;

            // T spanContext;

            var couldExtractTraceInfo = carrier.TryMapTo(operationName, out span);

            if (!couldExtractTraceInfo)
            {
                return false;
            }

            
            // span = NewSpan(spanContext, operationName, DateTime.Now);

            return true;
        }

        public ISpan StartSpan(StartSpanOptions startSpanOptions)
        {
            ISpan span;

            var rootSpanContext = _spanContextFactory.NewRootSpanContext();

            span = NewSpan(rootSpanContext, startSpanOptions.OperationName, startSpanOptions.StartTime);

            foreach (var tag in startSpanOptions.Tag)
            {
                span.SetTag(tag.Key, tag.Value);
            }

            return span;
        }

        public ISpan StartSpan(string operationName)
        {
            return StartSpan(new StartSpanOptions()
            {
                OperationName = operationName,
            });
        }

        internal ISpan NewSpan(T spanContext, string operationName, DateTime startTime)
        {
            return new Span<T>(this, _spanRecorder, spanContext, operationName, startTime);
        }
    }
}
