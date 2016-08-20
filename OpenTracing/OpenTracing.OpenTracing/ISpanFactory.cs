namespace OpenTracing
{
    public interface ISpanFactory
    {
        ISpan StartSpan(string operationName, StartSpanOptions startSpanOptions);
    }
}
