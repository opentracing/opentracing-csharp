using OpenTracing.Propagation;

namespace OpenTracing
{
    public interface ITracer
    {
        ISpan StartSpan(string operationName);
        ISpan StartSpan(StartSpanOptions startSpanOptions);

        void Inject(ISpan span, IInjectCarrier carrier);
        bool TryJoin(string operationName, IExtractCarrier carrier, out ISpan span);
    }
}
