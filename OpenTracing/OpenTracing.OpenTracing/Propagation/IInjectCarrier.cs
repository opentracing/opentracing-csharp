namespace OpenTracing.Propagation
{
    public interface IInjectCarrier<TFormat>
    {
        void MapFrom(TFormat format);
    }
}
