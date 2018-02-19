﻿using System;
using System.Collections.Generic;

namespace OpenTracing
{
    /// <summary>
    /// Represents the OpenTracing specification's Span contract. <seealso cref="IScope"/>
    /// <seealso cref="IScopeManager"/> <seealso cref="ISpanBuilder.Start"/> <seealso cref="ISpanBuilder.StartActive"/>
    /// </summary>
    public interface ISpan
    {
        /// <summary>
        /// Retrieve the associated SpanContext. This may be called at any time, including after calls to
        /// <see cref="ISpan.Finish()"/>.
        /// </summary>
        /// <returns>The SpanContext that encapsulates Span state that sould propagate across process boundaries.</returns>
        ISpanContext Context { get; }

        /// <summary>Set a key:value tag on the Span.</summary>
        ISpan SetTag(string key, string value);

        /// <summary>Same as <see cref="SetTag(string,string)"/> but for boolean values.</summary>
        ISpan SetTag(string key, bool value);

        /// <summary>Same as <see cref="SetTag(string,string)"/> but for numeric values.</summary>
        ISpan SetTag(string key, int value);

        /// <summary>Same as <see cref="SetTag(string,string)"/> but for numeric values.</summary>
        ISpan SetTag(string key, double value);

        /// <summary>
        /// Log key:value pairs to the Span with the current walltime.
        /// <para><em>CAUTIONARY NOTE:</em> not all Tracer implementations support key:value log fields end-to-end. Caveat emptor.</para>
        /// <para>
        /// A contrived example:
        /// <code>
        /// span.Log(
        /// new Dictionary{string, object}
        /// {
        ///   { "event", "soft error" },
        ///   { "type", "cache timeout" },
        ///   { "waited.millis", 1500 },
        /// });
        /// </code>
        /// </para>
        /// </summary>
        /// <param name="fields">
        /// key:value log fields. Tracer implementations should support String, numeric, and boolean values;
        /// some may also support arbitrary objects.
        /// </param>
        /// <returns>The Span, for chaining</returns>
        /// <seealso cref="Log(string)"/>
        ISpan Log(IDictionary<string, object> fields);

        /// <summary>
        /// Like <see cref="Log(IDictionary{string, object})"/>, but with an explicit timestamp.
        /// <para><em>CAUTIONARY NOTE:</em> not all Tracer implementations support key:value log fields end-to-end. Caveat emptor.</para>
        /// </summary>
        /// <param name="timestamp">
        /// The explicit timestamp for the log record. Must be greater than or equal to the Span's start
        /// timestamp.
        /// </param>
        /// <param name="fields">
        /// key:value log fields. Tracer implementations should support String, numeric, and boolean values;
        /// some may also support arbitrary objects.
        /// </param>
        /// <returns>The Span, for chaining</returns>
        /// <seealso cref="Log(DateTimeOffset, string)"/>
        ISpan Log(DateTimeOffset timestamp, IDictionary<string, object> fields);

        /// <summary>
        /// Record an event at the current walltime timestamp. Shorthand for
        /// <code>
        /// span.Log(new Dictionary{string, object}() { { "event", event } });
        /// </code>
        /// </summary>
        /// <param name="event">The event value; often a stable identifier for a moment in the Span lifecycle</param>
        /// <returns>The Span, for chaining</returns>
        ISpan Log(string @event);

        /// <summary>
        /// Record an event at a specific timestamp. Shorthand for
        /// <code>
        /// span.Log(timestamp, new Dictionary{string, object}() { { "event", event } });
        /// </code>
        /// </summary>
        /// <param name="timestamp">
        /// The explicit timestamp for the log record. Must be greater than or equal to the Span's start
        /// timestamp.
        /// </param>
        /// <param name="event">The event value; often a stable identifier for a moment in the Span lifecycle</param>
        /// <returns>The Span, for chaining</returns>
        ISpan Log(DateTimeOffset timestamp, string @event);

        /// <summary>
        /// Sets a baggage item in the Span (and its SpanContext) as a key:value pair. Baggage enables powerful
        /// distributed context propagation functionality where arbitrary application data can be carried along the full path of
        /// request execution throughout the system. Note 1: Baggage is only propagated to the future (recursive) children of this
        /// SpanContext. Note 2: Baggage is sent in-band with every subsequent local and remote calls, so this feature must be used
        /// with care.
        /// </summary>
        /// <returns>This Span instance, for chaining</returns>
        ISpan SetBaggageItem(string key, string value);

        /// <summary>The value of the baggage item identified by <paramref name="key"/>, or null if no such item could be found.</summary>
        string GetBaggageItem(string key);

        /// <summary>Sets the string name for the logical operation this Span represents.</summary>
        /// <returns>This Span instance, for chaining</returns>
        ISpan SetOperationName(string operationName);

        /// <summary>
        /// Sets the end timestamp to now and records the span.
        /// <para>
        /// With the exception of calls to <see cref="ISpan.Context"/>, this should be the last call made to the span
        /// instance. Future calls to <see cref="Finish()"/> are defined as noops, and future calls to methods other than
        /// <see cref="ISpan.Context"/> lead to undefined behavior (likely an exception).
        /// </para>
        /// </summary>
        /// <seealso cref="ISpan.Context"/>
        void Finish();

        /// <summary>
        /// Sets an explicit end timestamp and records the span.
        /// <para>
        /// With the exception of calls to Span.context(), this should be the last call made to the span instance, and to do
        /// otherwise leads to undefined behavior.
        /// </para>
        /// </summary>
        /// <param name="finishTimestamp">An explicit finish time</param>
        /// <seealso cref="ISpan.Context"/>
        void Finish(DateTimeOffset finishTimestamp);
    }
}
