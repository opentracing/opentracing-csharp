namespace OpenTracing.Propagation
{
    public interface IExtractCarrier<TFormat>
    {
        /// <summary>
        /// Extract returns the SpanContext propagated through the `carrier`. 
        /// The actual format of context depends on the type of TFormat.
        /// </summary>
        TFormat Extract();
    }
}