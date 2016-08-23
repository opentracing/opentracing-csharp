namespace OpenTracing.BasicTracer.Context
{
    public interface IContextMapper<TContext, TFormat>
    {
        ContextMapToResult<TContext> MapTo(TFormat data);
        TFormat MapFrom(TContext spanContext);
    }
}
