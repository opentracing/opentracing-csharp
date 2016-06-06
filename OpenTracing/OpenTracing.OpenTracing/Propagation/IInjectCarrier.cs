namespace OpenTracing.Propagation
{
    public interface IInjectCarrier<T>
    {
        void MapFrom(T spanContext);
    }
}
