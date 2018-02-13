using System.Collections.Generic;

namespace OpenTracing
{
    /// <summary>
    /// SpanContext represents Span state that must propagate to descendant Spans and across process boundaries.
    /// SpanContext is logically divided into two pieces: (1) the user-level "Baggage" that propagates across Span boundaries
    /// and (2) any Tracer-implementation-specific fields that are needed to identify or otherwise contextualize the associated
    /// Span instance(e.g., a { trace_id, span_id, sampled } tuple).
    /// </summary>
    /// <seealso cref="ISpan.SetBaggageItem"/>
    /// <seealso cref="ISpan.GetBaggageItem"/>
    public interface ISpanContext
    {
        /// <returns>All zero or more baggage items propagating along with the associated Span</returns>
        /// <seealso cref="ISpan.SetBaggageItem"/>
        /// <seealso cref="ISpan.GetBaggageItem"/>
        IEnumerable<KeyValuePair<string, string>> GetBaggageItems();
    }
}
