namespace OpenTracing.BasicTracer
{
    public interface ISpanContextFactory
    {
        SpanContext NewRootSpanContext();
        SpanContext NewChildSpanContext(SpanContext spanContext);
    }
}
