namespace OpenTracing.BasicTracer.Context
{
    public interface IContextMapper<T, TFormat>
    {
        ContextMapToResult<T> MapTo(TFormat data);
        TFormat MapFrom(T spanContext);
    }
}
