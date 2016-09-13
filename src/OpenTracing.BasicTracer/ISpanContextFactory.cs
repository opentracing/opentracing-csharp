using System;
using System.Collections.Generic;

namespace OpenTracing.BasicTracer
{
    public interface ISpanContextFactory
    {
        SpanContext CreateSpanContext(IList<Tuple<string, ISpanContext>> references);
    }
}
