using OpenTracing.Context;

namespace OpenTracing.Propagation
{
    public interface IInjectCarrier<T> where T : ISpanContext
    {
        void MapFrom(T spanContext);
    }
}
