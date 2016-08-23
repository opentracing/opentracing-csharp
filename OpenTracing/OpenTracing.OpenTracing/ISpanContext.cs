using System.Collections.Generic;

namespace OpenTracing
{


    /// <summary>
    /// ISpanContext represents Span state that must propagate to descendant Spans and across process
    /// boundaries (e.g., a <trace_id, span_id, sampled> tuple).
    /// </summary>
    public interface ISpanContext
    {
        /// <summary>
        /// GetBaggageItems grants access to an immutable copy of all baggage items 
        /// stored in the SpanContext.
        /// </summary>
        /// <returns></returns>
        IReadOnlyDictionary<string, string> GetBaggageItems();
    }
}
