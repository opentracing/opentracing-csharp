using OpenTracing.Context;

namespace OpenTracing.BasicTracer
{
    public interface ISpanRecorder<T> where T : ISpanContext
    {
        void RecordSpan(SpanData<T> span);
    }
}
