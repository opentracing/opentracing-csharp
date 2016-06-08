namespace OpenTracing.Propagation
{
    public interface IExtractCarrier
    {
        bool TryMapTo(string operationName, out ISpan span);
    }
}
