namespace OpenTracing.Propagation
{
    public interface IInjectCarrier
    {
        void MapFrom(ISpan span);
    }
}
