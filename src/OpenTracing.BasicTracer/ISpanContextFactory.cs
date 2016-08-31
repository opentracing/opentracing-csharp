using System.Collections.Generic;

namespace OpenTracing.BasicTracer
{
    public interface ISpanContextFactory
    {
        SpanContext CreateSpanContext(IList<SpanReference> references);
    }
}
