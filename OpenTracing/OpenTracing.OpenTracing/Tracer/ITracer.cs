using OpenTracing.OpenTracing.Span;
using OpenTracing.OpenTracing.Context;
using OpenTracing.OpenTracing.Propagation;

namespace OpenTracing.OpenTracing.Tracer
{
    public interface ITracer<T> where T : ISpanContext
    {
        ISpan<T> StartSpan(string operationName);
        ISpan<T> StartSpan(StartSpanOptions<T> startSpanOptions);

        void Inject(ISpan<T> span, IInjectCarrier<T> carrier);
        bool TryJoin(string operationName, IExtractCarrier<T> carrier, out ISpan<T> span);

        ISpanRecorder<T> SpanRecorder { get; }
    }
}
