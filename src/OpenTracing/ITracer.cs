using OpenTracing.Propagation;

namespace OpenTracing
{
    public interface ITracer
    {
        ISpan StartSpan(string operationName, StartSpanOptions options = null);

        void Inject(ISpanContext spanContext, IInjectCarrier carrier);

        ISpanContext Extract(IExtractCarrier carrier);
    }
}
