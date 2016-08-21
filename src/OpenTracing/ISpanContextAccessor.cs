namespace OpenTracing
{
    public interface ISpanContextAccessor
    {
        ISpanContext CurrentSpanContext { get; set; }
    }
}