using OpenTracing.OpenTracing.Context;

namespace OpenTracing.OpenTracing.Propagation
{
    public interface IInjectCarrier<T> where T : ISpanContext
    {
        void MapFrom(T spanContext);
    }
}
