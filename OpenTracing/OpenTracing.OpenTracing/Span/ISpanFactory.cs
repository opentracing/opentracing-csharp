using OpenTracing.OpenTracing.Context;
using OpenTracing.OpenTracing.Tracer;
using System;

namespace OpenTracing.OpenTracing.Span
{
    internal interface ISpanFactory<T> where T :ISpanContext
    {
        ISpan<T> NewSpan(ITracer<T> tracer, T spanContext, string operationName, DateTime startTime);
    }
}