using OpenTracing.BasicTracer.Context;
using System;
using System.Collections.Generic;

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

        public ISpan StartSpan(string operationName, StartSpanOptions startSpanOptions)
        {
            ISpan span;

            var rootSpanContext = _spanContextFactory.NewRootSpanContext();

            span = NewSpan(rootSpanContext, operationName, startSpanOptions.StartTime, startSpanOptions.References);

            foreach (var tag in startSpanOptions.Tag)
            {
                span.SetTag(tag.Key, tag.Value);
            }

            return span;
        }

        internal ISpan NewSpan(TContext spanContext, string operationName, DateTime startTime, List<SpanReference> references)
        {
            return new Span<TContext>(_spanRecorder, spanContext, operationName, startTime, references);
        }
    }
}
