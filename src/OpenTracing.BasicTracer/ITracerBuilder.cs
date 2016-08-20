using OpenTracing.BasicTracer.Context;

namespace OpenTracing.BasicTracer
{
    public interface ITracerBuilder<T>
    {
        ITracerBuilder<T> SetSpanContextFactory(ISpanContextFactory<ISpanContext> spanContextFactory);
        ITracer<T> BuildTracer();
    }
}
