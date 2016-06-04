namespace OpenTracing.Propagation
{
    public interface IExtractCarrier<T>
    {
        bool TryMapTo(out T spanContext);
    }
}
