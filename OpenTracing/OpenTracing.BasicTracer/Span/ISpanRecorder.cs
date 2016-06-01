using OpenTracing.BasicTracer.Span;
using OpenTracing.Context;

namespace OpenTracing.BasicTracer.Tracer
{
    public interface ISpanRecorder<T> where T : ISpanContext
    {
        void RecordSpan(SpanData<T> span);
    }
}
