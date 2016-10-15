using System;
using System.Collections.Generic;

namespace OpenTracing
{
    /// <summary>
    /// Span represents an active, un-finished span in the OpenTracing system.
    /// Spans are created by the <see cref="ITracer"/> interface.
    /// </summary>
    public interface ISpan : IDisposable
    {
        /// <summary>
        /// Returns the <see cref="ISpanContext"/> for this Span. Note that the return
        /// value of <see cref="Context"/> is still valid after a call to <see cref="Finish"/>, as is
        /// a call to <see cref="Context"/> after a call to <see cref="Finish"/>.
        /// </summary>
        ISpanContext Context { get; }

        /// <summary>
        /// Sets or changes the operation name.
        /// </summary>
        ISpan SetOperationName(string operationName);

        /// <summary>
        /// Adds a tag to the Span.
        /// </summary>
        /// <param name="key">If there is a pre-existing tag set for <paramref name="key"/>, it is overwritten.</param>
        /// <param name="value">The value to be stored.</param>
        /// <returns>The current <see cref="ISpan"/> instance for chaining.</returns>
        ISpan SetTag(string key, bool value);

        /// <summary>
        /// Adds a tag to the Span.
        /// </summary>
        /// <param name="key">If there is a pre-existing tag set for <paramref name="key"/>, it is overwritten.</param>
        /// <param name="value">The value to be stored.</param>
        /// <returns>The current <see cref="ISpan"/> instance for chaining.</returns>
        ISpan SetTag(string key, double value);

        /// <summary>
        /// Adds a tag to the Span.
        /// </summary>
        /// <param name="key">If there is a pre-existing tag set for <paramref name="key"/>, it is overwritten.</param>
        /// <param name="value">The value to be stored.</param>
        /// <returns>The current <see cref="ISpan"/> instance for chaining.</returns>
        ISpan SetTag(string key, string value);

        /// <summary>
        /// Log key:value pairs to the Span with the current timestamp.
        /// <para>Log key:value pairs to the Span with the current UTC timestamp.</para>
        /// <para>CAUTIONARY NOTE: not all Tracer implementations support key:value log fields end-to-end.
        /// Caveat emptor.</para>
        /// </summary>
        /// <param name="fields">
        ///   <para>key:value log fields. Tracer implementations should support string, numeric, and boolean values;
        ///   some may also support arbitrary objects.</para>
        ///   <para>Use <see cref="Fields"/> to easily pass key:value pairs without unique key validations - 
        ///   e.g. <c>Fields.Create("event", "soft error", "waited.millis", 1500)</c>.</para>
        /// </param>
        /// <returns>The current <see cref="ISpan"/> instance for chaining.</returns>
        /// <example>
        /// <code>
        /// // Using a Fields collection initializer (does not do unique key checks).
        /// span.Log(new Fields {
        ///     { "event", "soft error" },
        ///     { "type", "cache timeout" },
        ///     { "waited.millis", 1500 }
        /// });
        ///
        /// // Using the Fields factory methods (does not do unique key checks).
        /// span.Log(Fields.Create("event", "soft error", "type", "cache timeout", "waited.millis", 1500));
        ///
        /// // Using an arbitrary dictionary (does unique key checks).
        /// span.Log(new Dictionary&lt;string, object&gt; {
        ///     { "event", "soft error" },
        ///     { "type", "cache timeout" },
        ///     { "waited.millis", 1500 }
        /// });
        /// </code>
        /// </example>
        ISpan Log(IEnumerable<KeyValuePair<string, object>> fields);

        /// <summary>
        /// <para>Log key:value pairs to the Span with the a specific timestamp.</para>
        /// <para>CAUTIONARY NOTE: not all Tracer implementations support key:value log fields end-to-end.
        /// Caveat emptor.</para>
        /// </summary>
        /// <param name="timestamp">The explicit timestamp for the log record. Must be greater than or equal to the
        /// Span's start timestamp.</param>
        /// <param name="fields">
        ///   <para>key:value log fields. Tracer implementations should support string, numeric, and boolean values;
        ///   some may also support arbitrary objects.</para>
        ///   <para>Use <see cref="Fields"/> to easily pass key:value pairs without unique key validations - 
        ///   e.g. <c>Fields.Create("event", "soft error", "waited.millis", 1500)</c>.</para>
        /// </param>
        /// <returns>The current <see cref="ISpan"/> instance for chaining.</returns>
        /// <example>
        /// <code>
        /// // Using a Fields collection initializer (does not do unique key checks).
        /// span.Log(new Fields {
        ///     { "event", "soft error" },
        ///     { "type", "cache timeout" },
        ///     { "waited.millis", 1500 }
        /// });
        ///
        /// // Using the Fields factory methods (does not do unique key checks).
        /// span.Log(Fields.Create("event", "soft error", "type", "cache timeout", "waited.millis", 1500));
        ///
        /// // Using an arbitrary dictionary (does unique key checks).
        /// span.Log(new Dictionary&lt;string, object&gt; {
        ///     { "event", "soft error" },
        ///     { "type", "cache timeout" },
        ///     { "waited.millis", 1500 }
        /// });
        /// </code>
        /// </example>
        ISpan Log(DateTime timestamp, IEnumerable<KeyValuePair<string, object>> fields);

        /// <summary>
        /// Record an event at the current UTC timestamp.
        /// </summary>
        /// <param name="event">The event value; often a stable identifier for a moment in the Span lifecycle.</param>
        /// <returns>The current <see cref="ISpan"/> instance for chaining.</returns>
        ISpan Log(string @event);

        /// <summary>
        /// Record an event at the current UTC timestamp.
        /// </summary>
        /// <param name="timestamp">The explicit timestamp for the log record. Must be greater than or equal to the
        /// Span's start timestamp.</param>
        /// <param name="event">The event value; often a stable identifier for a moment in the Span lifecycle.</param>
        /// <returns>The current <see cref="ISpan"/> instance for chaining.</returns>
        ISpan Log(DateTime timestamp, string @event);

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
        /// <param name="finishTimestamp">
        ///   The timestamp which should be used for the finish time of the Span.
        ///   Use <see cref="DateTimeKind.Utc"/> whenever possible. The behavior of other kinds is not defined.
        /// </param>
        void Finish(DateTime? finishTimestamp = null);
    }
}