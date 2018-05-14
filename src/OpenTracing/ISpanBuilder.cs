using System;
using OpenTracing.Tag;

namespace OpenTracing
{
    public interface ISpanBuilder
    {
        /// <summary>A shorthand for <see cref="AddReference"/>(References.ChildOf, parent).
        /// <para>If parent == null, this is a noop.</para>
        /// </summary>
        ISpanBuilder AsChildOf(ISpanContext parent);

        /// <summary>A shorthand for <see cref="AddReference"/>(References.ChildOf, parent.Context).
        /// <para>If parent == null, this is a noop.</para>
        /// </summary>
        ISpanBuilder AsChildOf(ISpan parent);

        /// <summary>
        /// Add a reference from the span being built to a distinct (usually parent) span. May be called multiple times to
        /// represent multiple such references.
        /// <para>
        /// If
        /// <list type="bullet">
        /// <item>the <see cref="ITracer"/>'s <see cref="IScopeManager.Active"/> is not null, and </item>
        /// <item>no <b>explicit</b> references are added via <see cref="AddReference"/>, and </item>
        /// <item><see cref="IgnoreActiveSpan"/> is not invoked, </item>
        /// </list>
        /// ... then an inferred <see cref="References.ChildOf"/> reference is created to the <see cref="IScopeManager.Active"/>
        /// <see cref="ISpanContext"/> when either <see cref="StartActive(bool)"/> or <see cref="Start"/> is invoked.
        /// </para>
        /// </summary>
        /// <param name="referenceType">
        /// The reference type, typically one of the constants defined in <see cref="References"/>.
        /// </param>
        /// <param name="referencedContext">
        /// The <see cref="ISpanContext"/> being referenced; e.g., for a <see cref="References.ChildOf"/>
        /// reference, the referenecedContext is the parent. If referencedContext == null, the call to <see cref="AddReference"/> is
        /// a noop.
        /// </param>
        /// <seealso cref="References"/>
        ISpanBuilder AddReference(string referenceType, ISpanContext referencedContext);

        /// <summary>
        /// Do not create an implicit <see cref="References.ChildOf"/> reference to the
        /// <see cref="IScopeManager.Active"/>.
        /// </summary>
        ISpanBuilder IgnoreActiveSpan();

        /// <summary>Same as <see cref="ISpan.SetTag(string,string)"/>, but for the span being built.</summary>
        ISpanBuilder WithTag(string key, string value);

        /// <summary>Same as <see cref="ISpan.SetTag(string,bool)"/>, but for the span being built.</summary>
        ISpanBuilder WithTag(string key, bool value);

        /// <summary>Same as <see cref="ISpan.SetTag(string,int)"/>, but for the span being built.</summary>
        ISpanBuilder WithTag(string key, int value);

        /// <summary>Same as <see cref="ISpan.SetTag(string,double)"/>, but for the span being built.</summary>
        ISpanBuilder WithTag(string key, double value);

        /// <summary>Same as <see cref="ISpan.SetTag(BooleanTag,bool)"/>, but for the span being built.</summary>
        ISpanBuilder WithTag(BooleanTag tag, bool value);

        /// <summary>Same as <see cref="ISpan.SetTag(IntOrStringTag,string)"/>, but for the span being built.</summary>
        ISpanBuilder WithTag(IntOrStringTag tag, string value);

        /// <summary>Same as <see cref="ISpan.SetTag(IntTag,int)"/>, but for the span being built.</summary>
        ISpanBuilder WithTag(IntTag tag, int value);

        /// <summary>Same as <see cref="ISpan.SetTag(StringTag,string)"/>, but for the span being built.</summary>
        ISpanBuilder WithTag(StringTag tag, string value);

        /// <summary>Specify a timestamp of when the <see cref="ISpan"/> was started.</summary>
        ISpanBuilder WithStartTimestamp(DateTimeOffset timestamp);

        /// <summary>
        /// Same as <see cref="StartActive(bool)"/> with <c>finishSpanOnDispose: true</c>.
        /// <para/>
        /// Returns a newly started and activated <see cref="IScope"/>. The underlying span is finished
        /// automatically when the scope is disposed.
        /// <para/>
        /// The returned <see cref="IScope"/> supports using(). For example:
        /// <code>
        /// using (IScope scope = tracer.BuildSpan("...").StartActive())
        /// {
        ///     // (Do work)
        ///     scope.Span.SetTag( ... );  // etc, etc
        /// } // Span finishes automatically on Dispose.
        /// </code>
        /// <para>
        /// If
        /// <list type="bullet">
        /// <item>the <see cref="ITracer"/>'s <see cref="IScopeManager.Active"/> is not null, and </item>
        /// <item>no <b>explicit</b> references are added via <see cref="AddReference"/>, and </item>
        /// <item><see cref="IgnoreActiveSpan"/> is not invoked, </item>
        /// </list>
        /// ... then an inferred <see cref="References.ChildOf"/> reference is created to the <see cref="IScopeManager.Active"/>
        /// <see cref="ISpanContext"/> when either <see cref="Start"/> or <see cref="StartActive()"/> is invoked.
        /// </para>
        /// </summary>
        /// <returns>An <see cref="IScope"/>, already registered via the <see cref="IScopeManager"/>.</returns>
        /// <seealso cref="IScopeManager"/>
        /// <seealso cref="IScope"/>
        IScope StartActive();

        /// <summary>
        /// Returns a newly started and activated <see cref="IScope"/>.
        /// <para>
        /// The returned <see cref="IScope"/> supports using(). For example:
        /// <code>
        /// using (IScope scope = tracer.BuildSpan("...").StartActive(finishSpanOnDispose: true))
        /// {
        ///     // (Do work)
        ///     scope.Span.SetTag( ... );  // etc, etc
        /// }
        /// // Span finishes automatically only when 'finishSpanOnDispose' is true
        /// </code>
        /// </para>
        /// <para>
        /// If
        /// <list type="bullet">
        /// <item>the <see cref="ITracer"/>'s <see cref="IScopeManager.Active"/> is not null, and </item>
        /// <item>no <b>explicit</b> references are added via <see cref="AddReference"/>, and </item>
        /// <item><see cref="IgnoreActiveSpan"/> is not invoked, </item>
        /// </list>
        /// ... then an inferred <see cref="References.ChildOf"/> reference is created to the <see cref="IScopeManager.Active"/>
        /// <see cref="ISpanContext"/> when either <see cref="Start"/> or <see cref="StartActive(bool)"/> is invoked.
        /// </para>
        /// </summary>
        /// <param name="finishSpanOnDispose">
        /// Whether span should automatically be finished when <see cref="IDisposable.Dispose"/> is called.
        /// </param>
        /// <returns>An <see cref="IScope"/>, already registered via the <see cref="IScopeManager"/>.</returns>
        /// <seealso cref="IScopeManager"/>
        /// <seealso cref="IScope"/>
        IScope StartActive(bool finishSpanOnDispose);

        /// <summary>
        /// Like <see cref="StartActive(bool)"/>, but the returned <see cref="ISpan"/> has not been registered via the
        /// <see cref="IScopeManager"/>.
        /// </summary>
        /// <returns>
        /// The newly-started span instance, which has *not* been automatically registered via the
        /// <see cref="IScopeManager"/>.
        /// </returns>
        /// <seealso cref="StartActive(bool)"/>
        ISpan Start();
    }
}
