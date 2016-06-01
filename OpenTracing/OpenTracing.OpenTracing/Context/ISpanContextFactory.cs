namespace OpenTracing.Context
{
    public interface ISpanContextFactory<T> where T : ISpanContext
    {
        T NewRootSpanContext();
        T NewChildSpanContext(T spanContext);
    }
}
