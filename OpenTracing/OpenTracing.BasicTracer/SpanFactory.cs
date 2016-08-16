using OpenTracing.BasicTracer.Context;
using System;

namespace OpenTracing.BasicTracer
{
    public class SpanFactory<TContext> : ISpanFactory where TContext : Context.ISpanContext
    {
        private readonly ISpanContextFactory<TContext> _spanContextFactory;
        private ISpanRecorder<TContext> _spanRecorder;

        public SpanFactory(ISpanContextFactory<TContext> spanContextFactory, ISpanRecorder<TContext> spanRecorder)
        {
            _spanContextFactory = spanContextFactory;
            _spanRecorder = spanRecorder;
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

        internal ISpan NewSpan(TContext spanContext, string operationName, DateTime startTime)
        {
            return new Span<TContext>(_spanRecorder, spanContext, operationName, startTime);
        }
    }
}
