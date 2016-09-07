using System.Collections.Generic;

namespace OpenTracing.BasicTracer.Context
{
    public interface ISpanContextFactory<TContext>
    {
        TContext NewRootSpanContext();
        TContext NewChildSpanContext(IList<SpanReference> references);
    }
}
