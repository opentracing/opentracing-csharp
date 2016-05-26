using OpenTracing.OpenTracing.Context;
using OpenTracing.OpenTracing.Span;

namespace OpenTracing.OpenTracing.Tracer
{
    public interface ISpanRecorder<T> where T : ISpanContext
    {
        void RecordSpan(SpanData<T> span);
    }
}
