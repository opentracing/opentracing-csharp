using OpenTracing.Context;

namespace OpenTracing
{
    public interface ITraceBuilder<T> where T : ISpanContext
    {
        ITraceBuilder<T> SetSpanContextFactory(ISpanContextFactory<ISpanContext> spanContextFactory);
        ITracer<T> BuildTracer();
    }
}
