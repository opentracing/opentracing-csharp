namespace OpenTracing.BasicTracer
{
    public interface ISpanRecorder<T>
    {
        void RecordSpan(SpanData<T> span);
    }
}
