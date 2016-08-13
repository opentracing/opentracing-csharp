namespace OpenTracing.BasicTracer
{
    public interface ISpanRecorder<TContext>
    {
        void RecordSpan(SpanData<TContext> span);
    }
}
