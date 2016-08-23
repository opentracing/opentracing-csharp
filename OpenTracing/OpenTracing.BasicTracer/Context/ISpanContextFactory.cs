namespace OpenTracing.BasicTracer.Context
{
    public interface ISpanContextFactory<TContext>
    {
        TContext NewRootSpanContext();
        TContext NewChildSpanContext(TContext spanContext);
    }
}
