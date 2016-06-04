namespace OpenTracing.Propagation
{
    public interface IContextMapper<T, TFormat>
    {
        bool TryMapTo(TFormat data, out T spanContext);
        TFormat MapFrom(T spanContext);
    }
}
