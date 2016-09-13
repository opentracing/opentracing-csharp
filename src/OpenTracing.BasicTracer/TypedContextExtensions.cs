namespace OpenTracing.BasicTracer
{
    public static class TypedContextExtensions
    {
        public static SpanContext TypedContext(this ISpan span) => (SpanContext)span?.Context;

        public static SpanContext TypedContext(this ISpanContext context) => (SpanContext)context;
    }
}