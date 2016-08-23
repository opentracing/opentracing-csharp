namespace OpenTracing
{
    /// <summary>
    /// SpanContext represents Span state that must propagate to descendant Spans and across process
    /// boundaries (e.g., a [trace_id, span_id, sampled] tuple).
    /// </summary>
    public interface ISpanContext
    {
        /// <summary>
        /// <para>Sets a baggage item as a key/value pair.</para>
        /// </summary>
        /// <remarks>
        /// <para>Baggage enables powerful distributed context propagation functionality where arbitrary application data can be
        /// carried along the full path of request execution throughout the system.</para>
        /// <para>Note 1: Baggage is only propagated to the future (recursive) children of this SpanContext.</para>
        /// <para>Baggage is sent in-band with every subsequent local and remote calls, so this feature must be used with care.</para>
        /// </remarks>
        /// <param name="key">If there is a pre-existing item set for <paramref name="key"/>, it is overwritten.</param>
        /// <param name="value">The value that should be stored.</param>
        /// <returns>The current <see cref="ISpanContext"/> instance for chaining.</returns>
        ISpanContext SetBaggageItem(string key, string value);

        /// <summary>
        /// Returns the value of the baggage item identified by the given <paramref name="key"/>,
        /// or <c>null</c> if no such item could be found.
        /// </summary>
        /// <param name="key">The name of the key which was used to store the baggage item.</param>
        string GetBaggageItem(string key);
    }
}