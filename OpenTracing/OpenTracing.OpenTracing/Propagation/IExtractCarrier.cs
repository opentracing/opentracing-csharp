using OpenTracing.OpenTracing.Context;

namespace OpenTracing.OpenTracing.Propagation
{
    public interface IExtractCarrier<T> where T : ISpanContext
    {
        bool TryMapTo(out T spanContext);
    }
}
