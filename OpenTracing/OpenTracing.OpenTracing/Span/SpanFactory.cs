using OpenTracing.OpenTracing.Context;
using OpenTracing.OpenTracing.Tracer;
using System;

namespace OpenTracing.OpenTracing.Span
{
    internal class SpanFactory<T> : ISpanFactory<T> where T : ISpanContext
    {
        public SpanFactory()
        {
        }

        public ISpan<T> NewSpan(ITracer<T> tracer, T spanContext, string operationName, DateTime startTime)
        {
            return new Span<T>(tracer, this, spanContext, operationName, startTime);
        }
    }
}