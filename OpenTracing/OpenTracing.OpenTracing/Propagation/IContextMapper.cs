using OpenTracing.OpenTracing.Context;

namespace OpenTracing.OpenTracing.Propagation
{
    public interface IContextMapper<T, TFormat> where TFormat : IFormat where T : ISpanContext
    {
        bool TryMapTo(TFormat data, out T spanContext);
        TFormat MapFrom(T spanContext);
    }
}
