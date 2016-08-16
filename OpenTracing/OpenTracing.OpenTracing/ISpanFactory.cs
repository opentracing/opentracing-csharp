namespace OpenTracing
{
    public interface ISpanFactory
    {
        ISpan StartSpan(StartSpanOptions startSpanOptions);
    }
}
