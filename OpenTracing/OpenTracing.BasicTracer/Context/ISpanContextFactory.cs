namespace OpenTracing.BasicTracer.Context
{
    public interface ISpanContextFactory<T>
    {
        T NewRootSpanContext();
        T NewChildSpanContext(T spanContext);
    }
}
