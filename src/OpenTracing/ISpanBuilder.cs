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
        ISpanBuilder AsChildOf(ISpan parent);

        /// <summary>
        ///     Add a reference from the Span being built to a distinct (usually parent) Span. May be called multiples times
        ///     to represent multiple such References.
        ///     <para>
        ///         If
        ///         <list type="bullet">
        ///             <item>the <see cref="ITracer" />'s <see cref="IScopeManager.Active" /> is not null, and</item>
        ///             <item>no <b>explicit</b> references are added via <see cref="AddReference" />, and</item>
        ///             <item><see cref="IgnoreActiveSpan" /> is not invoked,</item>
        ///         </list>
        ///         ... then an inferred <see cref="References.ChildOf" /> reference is created to the
        ///         <see cref="IScopeManager.Active" /> <see cref="ISpanContext" /> when either <see cref="StartActive()" />
        ///         or <see cref="StartManual" /> is invoked.
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
        ///     <see cref="IScopeManager.Active" />.
        /// </summary>
        /// <returns></returns>
        ISpanBuilder IgnoreActiveSpan();

        /// <summary>
        ///     Same as <see cref="ISpan.SetTag(string,string)" />, but for the span being built.
        /// </summary>
        ISpanBuilder WithTag(string key, string value);

        /// <summary>
        ///     Same as <see cref="ISpan.SetTag(string,bool)" />, but for the span being built.
        /// </summary>
        ISpanBuilder WithTag(string key, bool value);

        /// <summary>
        ///     Same as <see cref="ISpan.SetTag(string,int)" />, but for the span being built.
        /// </summary>
        ISpanBuilder WithTag(string key, int value);

        /// <summary>
        ///     Same as <see cref="ISpan.SetTag(string,double)" />, but for the span being built.
        /// </summary>
        ISpanBuilder WithTag(string key, double value);

        /// <summary>
        ///     Specify a timestamp of when the Span was started
        /// </summary>
        ISpanBuilder WithStartTimestamp(DateTimeOffset startTime);

        /// <summary>
        ///     Returns a newly started and activated <see cref="IScope" />.
        ///     <para>
        ///         The returned <see cref="IScope" /> supports using(). For example:
        ///         <code>
        /// using (IScope scope = tracer.BuildSpan("...").StartActive())
        /// {
        ///     // (Do work)
        ///     scope.Span.SetTag( ... );  // etc, etc
        /// }
        /// // Span finishes automatically when the IScope is closed
        /// // following the default behavior of IScopeManager.Activate(ISpan)
        /// </code>
        ///     </para>
        ///     <para>
        ///         For detailed information, see <see cref="StartActive(bool)"/>
        ///     </para>
        ///     <para>
        ///         NOTE: <see cref="StartActive()" /> is a shorthand for
        ///         <code>tracer.ScopeManager.Activate(spanBuilder.StartManual())</code>
        ///     </para>
        /// </summary>
        /// <returns>An <see cref="IScope" />, already registered via the <see cref="IScopeManager" /></returns>
        /// <seealso cref="IScopeManager" />
        /// <seealso cref="IScope"/>
        /// <seealso cref="StartActive(bool)"/>
        IScope StartActive();

        /// <summary>
        ///     Returns a newly started and activated <see cref="IScope" />.
        ///     <para>
        ///         The returned <see cref="IScope" /> supports using(). For example:
        ///         <code>
        /// using (IScope scope = tracer.BuildSpan("...").StartActive(false))
        /// {
        ///     // (Do work)
        ///     scope.Span.SetTag( ... );  // etc, etc
        /// }
        /// // Span does not finish automatically when the Scope is closed as
        /// // 'finishOnClose' is false
        /// </code>
        ///     </para>
        ///     <para>
        ///         If
        ///         <list type="bullet">
        ///             <item>the <see cref="ITracer" />'s <see cref="IScopeManager.Active" /> is not null, and</item>
        ///             <item>no <b>explicit</b> references are added via <see cref="AddReference" />, and</item>
        ///             <item><see cref="IgnoreActiveSpan" /> is not invoked,</item>
        ///         </list>
        ///         ... then an inferred <see cref="References.ChildOf" /> reference is created to the
        ///         <see cref="IScopeManager.Active" /> <see cref="ISpanContext" /> when either
        ///         <see cref="StartManual" /> or <see cref="StartActive()" /> is invoked.
        ///     </para>
        ///     <para>
        ///         NOTE: <see cref="StartActive(bool)" /> is a shorthand for
        ///         <code>tracer.MakeActive(spanBuilder.StartManual(), finishSpanOnClose)</code>
        ///     </para>
        /// </summary>
        /// <param name="finishSpanOnClose">Whether span sould automatically be finished when <see cref="IDisposable.Dispose"/> is called</param>
        /// <returns>An <see cref="IScope" />, already registered via the <see cref="IScopeManager" /></returns>
        /// <seealso cref="IScopeManager" />
        /// <seealso cref="IScope" />
        IScope StartActive(bool finishSpanOnClose);

        /// <summary>
        ///     Like <see cref="StartActive()" />, but the returned <see cref="ISpan" /> has not been registered via the
        ///     <see cref="IScopeManager" />.
        /// </summary>
        /// <returns>
        ///     The newly-started Span instance, which as *not* been automatically registered
        ///     via the <see cref="IScopeManager" />
        /// </returns>
        /// <seealso cref="StartActive()" />
        ISpan StartManual();

        [Obsolete("Use StartManual or StartActive instead.")]
        ISpan Start();
    }
}