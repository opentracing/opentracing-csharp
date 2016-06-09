using OpenTracing.Propagation;

namespace OpenTracing
{
    public interface ITracer
    {
        ISpan StartSpan(string operationName);
        ISpan StartSpan(StartSpanOptions startSpanOptions);

        void Inject<T>(ISpan span, IInjectCarrier<T> carrier);
        bool TryJoin<T>(string operationName, IExtractCarrier<T> carrier, out ISpan span);
    }
}
