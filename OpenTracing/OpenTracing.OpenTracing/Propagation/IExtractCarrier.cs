using OpenTracing.Context;

namespace OpenTracing.Propagation
{
    public interface IExtractCarrier<T> where T : ISpanContext
    {
        bool TryMapTo(out T spanContext);
    }
}
