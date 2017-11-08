// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISpanBuilder.cs">
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

    public interface ISpanBuilder
    {
        /// <summary>
        ///     A shorthand for <see cref="AddReference" />(References.ChildOf, parent).
        ///     <para>
        ///         If parent == null, this is a noop.
        ///     </para>
        /// </summary>
        ISpanBuilder AsChildOf(ISpanContext parent);

        /// <summary>
        ///     A shorthand for <see cref="AddReference" />(References.ChildOf, parent.Context()).
        ///     <para>
        ///         If parent == null, this is a noop.
        ///     </para>
        /// </summary>
        ISpanBuilder AsChildOf<T>(IBaseSpan<T> parent)
            where T : IBaseSpan<T>;

        /// <summary>
        ///     Add a reference from the Span being built to a distinct (usually parent) Span. May be called multiples times
        ///     to represent multiple such References.
        ///     <para>
        ///         If
        ///         <list type="bullet">
        ///             <item>the <see cref="ITracer" />'s <see cref="IActiveSpanSource.ActiveSpan" /> is not null, and</item>
        ///             <item>no <b>explicit</b> references are added via <see cref="AddReference" />, and</item>
        ///             <item><see cref="IgnoreActiveSpan" /> is not invoked,</item>
        ///         </list>
        ///         ... then an inferred <see cref="References.ChildOf" /> reference is created to the
        ///         <see cref="IActiveSpanSource.ActiveSpan" /> <see cref="ISpanContext" /> when either <see cref="StartActive" />
        ///         or
        ///         <see cref="StartManual" /> is invoked.
        ///     </para>
        /// </summary>
        /// <param name="referenceType">
        ///     The reference type, typically one of the constants defined in <see cref="References" />
        /// </param>
        /// <param name="referencedContext">
        ///     The SpanContext being referenced; e.g., for a <see cref="References.ChildOf" /> reference, the
        ///     referenecedContext is the parent. If referencedContext==null, the call to <see cref="AddReference" /> is a noop.
        /// </param>
        /// <seealso cref="References" />
        ISpanBuilder AddReference(string referenceType, ISpanContext referencedContext);

        /// <summary>
        ///     Do not create an implicit <see cref="References.ChildOf" /> reference to othe
        ///     <see cref="IActiveSpanSource.ActiveSpan" />.
        /// </summary>
        /// <returns></returns>
        ISpanBuilder IgnoreActiveSpan();

        /// <summary>
        ///     Same as <see cref="IBaseSpan{T}.SetTag(string,string)" />, but for the span being built.
        /// </summary>
        ISpanBuilder WithTag(string key, string value);

        /// <summary>
        ///     Same as <see cref="IBaseSpan{T}.SetTag(string,bool)" />, but for the span being built.
        /// </summary>
        ISpanBuilder WithTag(string key, bool value);

        /// <summary>
        ///     Same as <see cref="IBaseSpan{T}.SetTag(string,int)" />, but for the span being built.
        /// </summary>
        ISpanBuilder WithTag(string key, int value);

        /// <summary>
        ///     Same as <see cref="IBaseSpan{T}.SetTag(string,double)" />, but for the span being built.
        /// </summary>
        ISpanBuilder WithTag(string key, double value);

        /// <summary>
        ///     Specify a timestamp of when the Span was started
        /// </summary>
        ISpanBuilder WithStartTimestamp(DateTimeOffset startTime);

        /// <summary>
        ///     Returns a newly started and activated <see cref="IActiveSpan" />.
        ///     <para>
        ///         The returned <see cref="IActiveSpan" /> supports using(). For example:
        ///         <code>
        /// using (ActiveSpan span = tracer.BuildSpan("...").StartActive())
        /// {
        ///     // (Do work)
        ///     span.SetTag( ... );  // etc, etc
        /// }  // Span finishes automatically unless deferred via <see cref="IActiveSpan.Capture" />
        /// </code>
        ///     </para>
        ///     <para>
        ///         If
        ///         <list type="bullet">
        ///             <item>the <see cref="ITracer" />'s <see cref="IActiveSpanSource.ActiveSpan" /> is not null, and</item>
        ///             <item>no <b>explicit</b> references are added via <see cref="AddReference" />, and</item>
        ///             <item><see cref="IgnoreActiveSpan" /> is not invoked,</item>
        ///         </list>
        ///         ... then an inferred <see cref="References.ChildOf" /> reference is created to the
        ///         <see cref="IActiveSpanSource.ActiveSpan" /> <see cref="ISpanContext" /> when either
        ///         <see cref="StartManual" /> or <see cref="StartActive" /> is invoked.
        ///     </para>
        ///     <para>
        ///         NOTE: <see cref="StartActive" /> is a shorthand for
        ///         <code>tracer.MakeActive(spanBuilder.StartManual())</code>
        ///     </para>
        /// </summary>
        /// <returns>An <see cref="IActiveSpan" />, already registered via the <see cref="IActiveSpanSource" /></returns>
        /// <seealso cref="IActiveSpanSource" />
        /// <seealso cref="IActiveSpan" />
        IActiveSpan StartActive();

        /// <summary>
        ///     Like <see cref="StartActive" />, but the returned <see cref="ISpan" /> has not been registered via the
        ///     <see cref="IActiveSpanSource" />.
        /// </summary>
        /// <returns>
        ///     The newly-started Span instance, which as *not* been automatically registered
        ///     via the <see cref="IActiveSpanSource" />
        /// </returns>
        /// <seealso cref="StartActive" />
        ISpan StartManual();

        [Obsolete("Use StartManual or StartActive instead.")]
        ISpan Start();
    }
}