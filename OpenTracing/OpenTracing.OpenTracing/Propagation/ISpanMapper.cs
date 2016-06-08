namespace OpenTracing.Propagation
{
    public interface ISpanMapper<TFormat>
    {
        bool TryMapTo(string operationName, TFormat data, out ISpan span);
        TFormat MapFrom(ISpan span);
    }
}
