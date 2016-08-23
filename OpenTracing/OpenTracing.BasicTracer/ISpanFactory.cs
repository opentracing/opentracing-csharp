namespace OpenTracing.BasicTracer
{
    public interface ISpanFactory
    {
        ISpan StartSpan(string operationName, StartSpanOptions startSpanOptions);
    }
}
