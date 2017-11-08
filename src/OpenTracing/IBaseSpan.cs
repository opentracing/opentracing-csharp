// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IBaseSpan.cs">
//   Copyright 2017-2018 The OpenTracing Authors
//   
//   Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except
//   in compliance with the License. You may obtain a copy of the License at
//   
//   http://www.apache.org/licenses/LICENSE-2.0
// 
//   Unless required by applicable law or agreed to in writing, software distributed under the License
//   is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
//   or implied. See the License for the specific language governing permissions and limitations under
//   the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OpenTracing
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    ///     <see cref="IBaseSpan{T}" /> represents the OpenTracing specification's span contract with the exception of methods
    ///     to finish
    ///     said span. For those, either use <see cref="ISpan.Finish()" /> or <see cref="IActiveSpan.Deactivate" /> depending
    ///     on the
    ///     programming model.
    /// </summary>
    /// <seealso cref="ISpan" />
    /// <seealso cref="IActiveSpan" />
    /// <see cref="ISpanBuilder.StartManual" />
    /// <see cref="ISpanBuilder.StartActive" />
    public interface IBaseSpan<T>
        where T : IBaseSpan<T>
    {
        /// <summary>
        ///     Retrieve the associated SpanContext.
        ///     This may be called at any time, including after calls to <see cref="ISpan.Finish()" />.
        /// </summary>
        /// <returns>The SpanContext that encapsulates Span state that sould propagate across process boundaries.</returns>
        ISpanContext Context();

        /// <summary>
        ///     Set a key:value tag on the Span.
        /// </summary>
        T SetTag(string key, string value);

        /// <summary>
        ///     Same as <see cref="SetTag(string,string)" /> but for boolean values.
        /// </summary>
        T SetTag(string key, bool value);

        /// <summary>
        ///     Same as <see cref="SetTag(string,string)" /> but for numeric values.
        /// </summary>
        T SetTag(string key, int value);

        /// <summary>
        ///     Same as <see cref="SetTag(string,string)" /> but for numeric values.
        /// </summary>
        T SetTag(string key, double value);

        /// <summary>
        ///     Log key:value pairs to the Span with the current walltime.
        ///     <para>
        ///         <em>CAUTIONARY NOTE:</em> not all Tracer implementations support key:value log fields end-to-end.
        ///         Caveat emptor.
        ///     </para>
        ///     <para>
        ///         A contrived example:
        ///         <code>
        /// span.Log(
        /// new Dictionary{string, object}
        /// {
        ///   { "event", "soft error" },
        ///   { "type", "cache timeout" },
        ///   { "waited.millis", 1500 },
        /// });
        /// </code>
        ///     </para>
        /// </summary>
        /// <param name="fields">
        ///     key:value log fields. Tracer implementations should support String, numeric, and boolean values;
        ///     some may also support arbitrary objects.
        /// </param>
        /// <returns>The Span, for chaining</returns>
        /// <seealso cref="Log(string)" />
        T Log(IReadOnlyDictionary<string, object> fields);

        /// <summary>
        ///     Like <see cref="Log(System.Collections.Generic.IReadOnlyDictionary{string,object})" />, but with an explicit
        ///     timestamp.
        ///     <para>
        ///         <em>CAUTIONARY NOTE:</em> not all Tracer implementations support key:value log fields end-to-end.
        ///         Caveat emptor.
        ///     </para>
        /// </summary>
        /// <param name="timestamp">
        ///     The explicit timestamp for the log record. Must be greater than or equal to the
        ///     Span's start timestamp.
        /// </param>
        /// <param name="fields">
        ///     key:value log fields. Tracer implementations should support String, numeric, and boolean values;
        ///     some may also support arbitrary objects.
        /// </param>
        /// <returns>The Span, for chaining</returns>
        /// <seealso cref="Log(DateTimeOffset, string)" />
        T Log(DateTimeOffset timestamp, IReadOnlyDictionary<string, object> fields);

        /// <summary>
        ///     Record an event at the current walltime timestamp.
        ///     Shorthand for
        ///     <code>
        /// span.Log(new Dictionary{string, object}() { { "event", event } });
        /// </code>
        /// </summary>
        /// <param name="event">The event value; often a stable identifier for a moment in the Span lifecycle</param>
        /// <returns>The Span, for chaining</returns>
        T Log(string @event);

        /// <summary>
        ///     Record an event at a specific timestamp.
        ///     Shorthand for
        ///     <code>
        /// span.Log(timestamp, new Dictionary{string, object}() { { "event", event } });
        /// </code>
        /// </summary>
        /// <para name="timestap">
        ///     The explicit timestamp for the log record. Must be greater than or equal to the
        ///     Span's start timestamp.
        /// </para>
        /// <param name="event">The event value; often a stable identifier for a moment in the Span lifecycle</param>
        /// <returns>The Span, for chaining</returns>
        T Log(DateTimeOffset timestamp, string @event);

        /// <summary>
        ///     Sets a baggage item in the Span (and its SpanContext) as a key:value pair.
        ///     Baggage enables powerful distributed context propagation functionality where arbitrary application data can be
        ///     carried along the full path of request execution throughout the system.
        ///     Note 1: Baggage is only propagated to the future (recursive) children of this SpanContext.
        ///     Note 2: Baggage is sent in-band with every subsequent local and remote calls, so this feature must be used with
        ///     care.
        /// </summary>
        /// <returns>The Span, for chaining</returns>
        T SetBaggageItem(string key, string value);

        /// <summary>
        ///     The value of the baggage item identified by <paramref name="key" />, or null if no such item could be found.
        /// </summary>
        string GetBaggaggeItem(string key);

        /// <summary>
        ///     Sets the string name for the logical operation this Span represents.
        /// </summary>
        /// <returns>The Span, for chaining</returns>
        T SetOperationName(string operationName);
    }
}