namespace OpenTracing
{
    using System.Collections.Generic;

    /// <summary>
    /// <para>A SpanContext represents span state that must propagate to descendant spans and across process boundaries.</para>
    /// <para>SpanContext is logically divided into two pieces: (1) the user-level "Baggage" that propagates across span
    /// boundaries and (2) any Tracer-implementation-specific fields that are needed to identify or otherwise contextualize
    /// the associated span instance (e.g., a [trace_id, span_id, sampled] tuple).</para>
    /// </summary>
    public interface ISpanContext
    {
        /// <summary>
        /// GetBaggageItems grants access to an immutable copy of all baggage items
        /// stored in the <see cref="ISpanContext"/>.
        /// </summary>
        IEnumerable<KeyValuePair<string, string>> GetBaggageItems();
    }
}