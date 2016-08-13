namespace OpenTracing.Propagation
{
    public interface IExtractCarrier<TFormat>
    {
        ExtractCarrierResult<TFormat> Extract();
    }
}
