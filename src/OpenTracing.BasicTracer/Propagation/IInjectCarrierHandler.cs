using OpenTracing.Propagation;

namespace OpenTracing.BasicTracer.Propagation
{
    public interface IInjectCarrierHandler
    {
    }
    
    public interface IInjectCarrierHandler<TCarrier> : IInjectCarrierHandler where TCarrier : IInjectCarrier
    {
        void MapContextToCarrier(SpanContext context, TCarrier carrier);
    }
}