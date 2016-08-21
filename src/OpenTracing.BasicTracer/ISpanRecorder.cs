namespace OpenTracing.BasicTracer
{
    public interface ISpanRecorder
    {
        void RecordSpan(SpanData span);
    }
}
