using OpenTracing.BasicTracer.Context;

namespace OpenTracing.BasicTracer
{
    public interface ITracerBuilder<TContext>
    {
        ITracerBuilder<TContext> SetSpanContextFactory(ISpanContextFactory<TContext> spanContextFactory);
        ITracer BuildTracer();
    }
}
