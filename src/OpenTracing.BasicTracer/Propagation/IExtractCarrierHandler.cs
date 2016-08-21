using OpenTracing.Propagation;

namespace OpenTracing.BasicTracer.Propagation
{
    public interface IExtractCarrierHandler
    {
    }

    public interface IExtractCarrierHandler<TCarrier> : IExtractCarrierHandler where TCarrier : IExtractCarrier
    {
        SpanContext MapCarrierToContext(TCarrier carrier);
    }
}