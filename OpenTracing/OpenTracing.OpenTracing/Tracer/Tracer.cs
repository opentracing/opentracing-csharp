using OpenTracing.OpenTracing.Context;
using OpenTracing.OpenTracing.Span;
using System;
using OpenTracing.OpenTracing.Propagation;

namespace OpenTracing.OpenTracing.Tracer
{
    public class Tracer<T> : ITracer<T> where T : ISpanContext
    {
        private readonly ISpanContextFactory<T> _spanContextFactory;
        private readonly ISpanFactory<T> _spanFactory;
        public ISpanRecorder<T> SpanRecorder { get; private set; }

        internal Tracer(ISpanContextFactory<T> spanContextFactory, ISpanFactory<T> spanFactory, ISpanRecorder<T> spanRecorder)
        {
            _spanContextFactory = spanContextFactory;
            _spanFactory = spanFactory;
            SpanRecorder = spanRecorder;
        }

        public void Inject(ISpan<T> span, IInjectCarrier<T> carrier)
        {
            carrier.MapFrom(span.GetSpanContext());
        }

        public bool TryJoin(string operationName, IExtractCarrier<T> carrier, out ISpan<T> span)
        {
            span = null;

            T spanContext;

            var couldExtractTraceInfo = carrier.TryMapTo(out spanContext);

            if (!couldExtractTraceInfo)
            {
                return false;
            }

            span = _spanFactory.NewSpan(this, spanContext, operationName, DateTime.Now);

            return true;
        }

        public ISpan<T> StartSpan(StartSpanOptions<T> startSpanOptions)
        {
            ISpan<T> span;

            if (startSpanOptions.ParentContext == null)
            {

                var rootSpanContext = _spanContextFactory.NewRootSpanContext();

                span = _spanFactory.NewSpan(this, rootSpanContext, startSpanOptions.OperationName, startSpanOptions.StartTime);
            }
            else
            {
                var childSpanContext = _spanContextFactory.NewChildSpanContext(startSpanOptions.ParentContext);
                span = _spanFactory.NewSpan(this, childSpanContext, startSpanOptions.OperationName, startSpanOptions.StartTime);
            }

            foreach (var tag in startSpanOptions.Tag)
            {
                span.SetTag(tag.Key, tag.Value);
            }

            return span;
        }

        public ISpan<T> StartSpan(string operationName)
        {
            return StartSpan(new StartSpanOptions<T>()
            {
                OperationName = operationName,
            });
        }
    }
}
