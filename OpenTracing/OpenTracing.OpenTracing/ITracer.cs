using OpenTracing.Context;
using OpenTracing.Propagation;

namespace OpenTracing
{
    public interface ITracer<T>
    {
        ISpan<T> StartSpan(string operationName);
        ISpan<T> StartSpan(StartSpanOptions startSpanOptions);

        void Inject(ISpan<T> span, IInjectCarrier<T> carrier);
        bool TryJoin(string operationName, IExtractCarrier<T> carrier, out ISpan<T> span);
    }
}
