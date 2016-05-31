using OpenTracing.BasicTracer.Span;
using OpenTracing.OpenTracing.Context;
using OpenTracing.OpenTracing.Propagation;
using OpenTracing.OpenTracing.Span;
using OpenTracing.OpenTracing.Tracer;
using System;

namespace OpenTracing.BasicTracer.Tracer
{
    public class Tracer<T> : ITracer<T> where T : ISpanContext
    {
        private readonly ISpanContextFactory<T> _spanContextFactory;
        private ISpanRecorder<T> _spanRecorder;

        internal Tracer(ISpanContextFactory<T> spanContextFactory, ISpanRecorder<T> spanRecorder)
        {
            _spanContextFactory = spanContextFactory;
            _spanRecorder = spanRecorder;
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

            span = NewSpan(spanContext, operationName, DateTime.Now);

            return true;
        }

        public ISpan<T> StartSpan(StartSpanOptions startSpanOptions)
        {
            ISpan<T> span;

            var rootSpanContext = _spanContextFactory.NewRootSpanContext();

            span = NewSpan(rootSpanContext, startSpanOptions.OperationName, startSpanOptions.StartTime);

            foreach (var tag in startSpanOptions.Tag)
            {
                span.SetTag(tag.Key, tag.Value);
            }

            return span;
        }

        public ISpan<T> StartSpan(string operationName)
        {
            return StartSpan(new StartSpanOptions()
            {
                OperationName = operationName,
            });
        }

        private ISpan<T> NewSpan(T spanContext, string operationName, DateTime startTime)
        {
            return new Span<T>(this, _spanRecorder, spanContext, operationName, startTime);
        }
    }
}
