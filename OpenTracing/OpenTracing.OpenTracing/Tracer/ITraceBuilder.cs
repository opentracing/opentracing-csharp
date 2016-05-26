using OpenTracing.OpenTracing.Context;

namespace OpenTracing.OpenTracing.Tracer
{
    public interface ITraceBuilder<T> where T : ISpanContext
    {
        ITraceBuilder<T> SetSpanContextFactory(ISpanContextFactory<ISpanContext> spanContextFactory);
        ITracer<T> BuildTracer();
    }
}
