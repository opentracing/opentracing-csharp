namespace OpenTracing.BasicTracer.Context
{
    public interface IContextMapper<T, TFormat>
    {
        bool TryMapTo(TFormat data, out T spanContext);
        TFormat MapFrom(T spanContext);
    }
}
