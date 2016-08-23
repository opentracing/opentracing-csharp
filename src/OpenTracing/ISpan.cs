using System;

namespace OpenTracing
{
    /// <summary>
    /// Span represents an active, un-finished span in the OpenTracing system.
    /// Spans are created by the <see cref="ITracer"/> interface.
    /// </summary>
    public interface ISpan : IDisposable
    {
        // TODO IDisposable behavior?
        // TODO SetOperationName() ?
        // TODO DateTimeOffset vs ticks ?

        /// <summary>
        /// Returns the <see cref="ISpanContext"/> for this Span. Note that the return
        /// value of <see cref="Context"/> is still valid after a call to <see cref="Finish"/>, as is
        /// a call to <see cref="Context"/> after a call to <see cref="Finish"/>.
        /// </summary>
        ISpanContext Context { get; }

        /// <summary>
        /// Adds a tag to the Span.
        /// </summary>
        /// <param name="key">If there is a pre-existing tag set for <paramref name="key"/>, it is overwritten.</param>
        /// <param name="value">
        /// Tag values can be numeric types, strings, or bools. The behavior of other tag value types is undefined 
        /// at the OpenTracing level. If a tracing system does not know how to handle a particular value type, it
        /// may ignore the tag, but shall not panic.
        /// </param>
        /// <returns>The current <see cref="ISpan"/> instance for chaining.</returns>
        ISpan SetTag(string key, object value);

        /// <summary>
        /// Records an event with optional payload data for this Span.
        /// </summary>
        /// <param name="eventName">Name of the event.</param>
        /// <param name="payload">An optional payload object.</param>
        /// <returns>The current <see cref="ISpan"/> instance for chaining.</returns>
        ISpan LogEvent(string eventName, object payload = null);

        /// <summary>
        /// Records an event with optional payload data for this Span.
        /// </summary>
        /// <param name="timestamp">The timestamp at which the event occured.</param>
        /// <param name="eventName">Name of the event.</param>
        /// <param name="payload">An optional payload object.</param>
        /// <returns>The current <see cref="ISpan"/> instance for chaining.</returns>
        ISpan LogEvent(DateTimeOffset timestamp, string eventName, object payload = null);

        /// <summary>
        /// <para>Sets a baggage item in the Span (and its SpanContext) as a key/value pair.</para>
        /// </summary>
        /// <remarks>
        /// <para>Baggage enables powerful distributed context propagation functionality where arbitrary application data can be
        /// carried along the full path of request execution throughout the system.</para>
        /// <para>Note 1: Baggage is only propagated to the future (recursive) children of this SpanContext.</para>
        /// <para>Baggage is sent in-band with every subsequent local and remote calls, so this feature must be used with care.</para>
        /// </remarks>
        /// <param name="key">If there is a pre-existing item set for <paramref name="key"/>, it is overwritten.</param>
        /// <param name="value">The value that should be stored.</param>
        /// <returns>The current <see cref="ISpan"/> instance for chaining.</returns>
        ISpan SetBaggageItem(string key, string value);

        /// <summary>
        /// Returns the value of the baggage item identified by the given <paramref name="key"/>,
        /// or <c>null</c> if no such item could be found.
        /// </summary>
        /// <param name="key">The name of the key which was used to store the baggage item.</param>
        string GetBaggageItem(string key);


        /// <summary>
        /// <para>Sets the end timestamp and finalizes Span state.</para>
        /// <para>
        /// With the exception of calls to <see cref="Context"/> (which are always allowed),
        /// <see cref="Finish"/> must be the last call made to any span instance, and to do
        /// otherwise leads to undefined behavior.
        /// </para>
        /// </summary>
        /// <param name="finishTimestamp">The timestamp which should be used for the finish time of the Span.</param>
        void Finish(DateTimeOffset? finishTimestamp = null);
    }
}