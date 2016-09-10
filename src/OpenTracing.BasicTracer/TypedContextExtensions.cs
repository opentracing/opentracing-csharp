using OpenTracing.BasicTracer.Context;

namespace OpenTracing.BasicTracer
{
    public static class TypedContextExtensions
    {
        public static SpanContext TypedContext(this ISpan span) => (SpanContext)span?.Context;

        public static SpanContext TypedContext(this SpanReference reference) => (SpanContext)reference?.Context;
    }
}