using System;
using System.Collections.Generic;

namespace OpenTracing
{
    /// <summary>
    /// Represents an in-flight span in the opentracing system.
    /// Spans are created by the <see cref="ITracer"/> interface.
    /// </summary>
    public interface ISpan : IDisposable
    {
        /// <summary>
        /// Returns the <see cref="ISpanContext"/> that encapsulates span state that should propagate across process boundaries.
        /// Note that the return value of <see cref="Context"/> is still valid after a call to <see cref="Finish"/>, as is
        /// a call to <see cref="Context"/> after a call to <see cref="Finish"/>.
        /// </summary>
        ISpanContext Context { get; }

        /// <summary>
        /// Sets or changes the operation name.
        /// </summary>
        ISpan SetOperationName(string operationName);

        /// <summary>
        /// Adds a tag to the span.
        /// </summary>
        /// <param name="key">If there is a pre-existing tag set for <paramref name="key"/>, it is overwritten.</param>
        /// <param name="value">The value to be stored.</param>
        /// <returns>The current <see cref="ISpan"/> instance for chaining.</returns>
        ISpan SetTag(string key, bool value);

        /// <summary>
        /// Adds a tag to the span.
        /// </summary>
        /// <param name="key">If there is a pre-existing tag set for <paramref name="key"/>, it is overwritten.</param>
        /// <param name="value">The value to be stored.</param>
        /// <returns>The current <see cref="ISpan"/> instance for chaining.</returns>
        ISpan SetTag(string key, double value);

        /// <summary>
        /// Adds a tag to the span.
        /// </summary>
        /// <param name="key">If there is a pre-existing tag set for <paramref name="key"/>, it is overwritten.</param>
        /// <param name="value">The value to be stored.</param>
        /// <returns>The current <see cref="ISpan"/> instance for chaining.</returns>
        ISpan SetTag(string key, int value);

        /// <summary>
        /// Adds a tag to the span.
        /// </summary>
        /// <param name="key">If there is a pre-existing tag set for <paramref name="key"/>, it is overwritten.</param>
        /// <param name="value">The value to be stored.</param>
        /// <returns>The current <see cref="ISpan"/> instance for chaining.</returns>
        ISpan SetTag(string key, string value);

        /// <summary>
        /// <para>Log key:value pairs to the span with the current timestamp.</para>
        /// <para>CAUTIONARY NOTE: Not all Tracer implementations support key:value log fields end-to-end.
        /// It is possible to pass a list of key:value pairs instead of a dictionary. However, the behavior
        /// for lists with duplicate keys is undefined.
        /// Caveat emptor.</para>
        /// </summary>
        /// <param name="fields">
        ///   <para>key:value log fields. Tracer implementations should support string, numeric, and boolean values;
        ///   some may also support arbitrary objects.</para>
        ///   <para>The behavior for lists with duplicate keys is undefined.</param>
        /// </param>
        /// <returns>The current <see cref="ISpan"/> instance for chaining.</returns>
        /// <example>
        /// <code>
        /// span.Log(new Dictionary&lt;string, object&gt; {
        ///     { "event", "soft error" },
        ///     { "type", "cache timeout" },
        ///     { "waited.millis", 1500 }
        /// });
        /// </code>
        /// </example>
        ISpan Log(IEnumerable<KeyValuePair<string, object>> fields);

        /// <summary>
        /// <para>Log key:value pairs to the span with an explicit timestamp.</para>
        /// <para>CAUTIONARY NOTE: Not all Tracer implementations support key:value log fields end-to-end.
        /// It is possible to pass a list of key:value pairs instead of a dictionary. However, the behavior
        /// for lists with duplicate keys is undefined.
        /// Caveat emptor.</para>
        /// </summary>
        /// <param name="timestamp">The explicit timestamp for the log record. Must be greater than or equal to the
        /// span's start timestamp.</param>
        /// <param name="fields">
        ///   <para>key:value log fields. Tracer implementations should support string, numeric, and boolean values;
        ///   some may also support arbitrary objects.</para>
        ///   <para>The behavior for lists with duplicate keys is undefined.</param>
        /// </param>
        /// <returns>The current <see cref="ISpan"/> instance for chaining.</returns>
        /// <example>
        /// <code>
        /// span.Log(new Dictionary&lt;string, object&gt; {
        ///     { "event", "soft error" },
        ///     { "type", "cache timeout" },
        ///     { "waited.millis", 1500 }
        /// });
        /// </code>
        /// </example>
        ISpan Log(DateTime timestamp, IEnumerable<KeyValuePair<string, object>> fields);

        /// <summary>
        /// Record an event at the current timestamp.
        /// </summary>
        /// <param name="eventName">The event value; often a stable identifier for a moment in the span lifecycle.</param>
        /// <returns>The current <see cref="ISpan"/> instance for chaining.</returns>
        ISpan Log(string eventName);

        /// <summary>
        /// Record an event at an explicit timestamp.
        /// </summary>
        /// <param name="timestamp">The explicit timestamp for the log record. Must be greater than or equal to the
        /// span's start timestamp.</param>
        /// <param name="eventName">The event value; often a stable identifier for a moment in the span lifecycle.</param>
        /// <returns>The current <see cref="ISpan"/> instance for chaining.</returns>
        ISpan Log(DateTime timestamp, string eventName);

        /// <summary>
        /// <para>Sets a baggage item in the span (and its SpanContext) as a key/value pair.</para>
        /// </summary>
        /// <remarks>
        /// <para>Baggage enables powerful distributed context propagation functionality where arbitrary application data can be
        /// carried along the full path of request execution throughout the system.</para>
        /// <para>Note 1: Baggage is only propagated to the future (recursive) children of this SpanContext.</para>
        /// <para>Note 2: Baggage is sent in-band with every subsequent local and remote calls, so this feature must be used with care.</para>
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
        /// <para>Sets the end timestamp to now and records the span.</para>
        /// <para>
        /// With the exception of calls to <see cref="Context"/> (which are always allowed),
        /// this should be the last call made to the span instance, and to do otherwise
        /// leads to undefined behavior.
        /// </para>
        /// </summary>
        void Finish();

        /// <summary>
        /// <para>Sets an explicit end timestamp and records the span.</para>
        /// <para>
        /// With the exception of calls to <see cref="Context"/> (which are always allowed),
        /// this should be the last call made to the span instance, and to do otherwise
        /// leads to undefined behavior.
        /// </para>
        /// <param name="finishTimestamp">
        /// An explicit finish timestamp.
        /// The behavior for kinds other than <see cref="DateTimeKind.Utc"/> is undefined.
        /// </param>
        /// </summary>
        void Finish(DateTime finishTimestamp);
    }
}