namespace OpenTracing.Propagation
{
    public interface IExtractCarrier<T>
    {
        ExtractCarrierResult<T> Extract();
    }
}
