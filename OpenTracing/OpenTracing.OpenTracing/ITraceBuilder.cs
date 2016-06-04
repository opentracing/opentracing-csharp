using OpenTracing.Context;

namespace OpenTracing
{
    public interface ITraceBuilder<T>
    {
        ITraceBuilder<T> SetSpanContextFactory(ISpanContextFactory<ISpanContext> spanContextFactory);
        ITracer<T> BuildTracer();
    }
}
