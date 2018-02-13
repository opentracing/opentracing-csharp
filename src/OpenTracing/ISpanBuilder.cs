using System;

namespace OpenTracing
{
    public interface ISpanBuilder
    {
        /// <summary>A shorthand for <see cref="AddReference"/>(References.ChildOf, parent).
        /// <para>If parent == null, this is a noop.</para>
        /// </summary>
        ISpanBuilder AsChildOf(ISpanContext parent);

        /// <summary>A shorthand for <see cref="AddReference"/>(References.ChildOf, parent.Context()).
        /// <para>If parent == null, this is a noop.</para>
        /// </summary>
        ISpanBuilder AsChildOf(ISpan parent);

        /// <summary>
        /// Add a reference from the Span being built to a distinct (usually parent) Span. May be called multiples times to
        /// represent multiple such References.
        /// <para>
        /// If
        /// <list type="bullet">
        /// <item>the <see cref="ITracer"/>'s <see cref="IScopeManager.Active"/> is not null, and</item>
        /// <item>no <b>explicit</b> references are added via <see cref="AddReference"/>, and</item>
        /// <item><see cref="IgnoreActiveSpan"/> is not invoked,</item>
        /// </list>
        /// ... then an inferred <see cref="References.ChildOf"/> reference is created to the <see cref="IScopeManager.Active"/>
        /// <see cref="ISpanContext"/> when either <see cref="StartActive"/> or <see cref="Start"/> is invoked.
        /// </para>
        /// </summary>
        /// <param name="referenceType">
        /// The reference type, typically one of the constants defined in <see cref="References"/>
        /// </param>
        /// <param name="referencedContext">
        /// The SpanContext being referenced; e.g., for a <see cref="References.ChildOf"/>
        /// reference, the referenecedContext is the parent. If referencedContext==null, the call to <see cref="AddReference"/> is
        /// a noop.
        /// </param>
        /// <seealso cref="References"/>
        ISpanBuilder AddReference(string referenceType, ISpanContext referencedContext);

        /// <summary>
        /// Do not create an implicit <see cref="References.ChildOf"/> reference to othe
        /// <see cref="IScopeManager.Active"/>.
        /// </summary>
        /// <returns></returns>
        ISpanBuilder IgnoreActiveSpan();

        /// <summary>Same as <see cref="ISpan.SetTag(string,string)"/>, but for the span being built.</summary>
        ISpanBuilder WithTag(string key, string value);

        /// <summary>Same as <see cref="ISpan.SetTag(string,bool)"/>, but for the span being built.</summary>
        ISpanBuilder WithTag(string key, bool value);

        /// <summary>Same as <see cref="ISpan.SetTag(string,int)"/>, but for the span being built.</summary>
        ISpanBuilder WithTag(string key, int value);

        /// <summary>Same as <see cref="ISpan.SetTag(string,double)"/>, but for the span being built.</summary>
        ISpanBuilder WithTag(string key, double value);

        /// <summary>Specify a timestamp of when the <see cref="ISpan"/> was started.</summary>
        ISpanBuilder WithStartTimestamp(DateTimeOffset timestamp);

        /// <summary>
        /// Returns a newly started and activated <see cref="IScope"/>.
        /// <para>
        /// The returned <see cref="IScope"/> supports using(). For example:
        /// <code>
        /// using (IScope scope = tracer.BuildSpan("...").StartActive(false))
        /// {
        ///     // (Do work)
        ///     scope.Span.SetTag( ... );  // etc, etc
        /// }
        /// // Span does not finish automatically when the Scope is closed as
        /// // 'finishOnClose' is false
        /// </code>
        /// </para>
        /// <para>
        /// If
        /// <list type="bullet">
        /// <item>the <see cref="ITracer"/>'s <see cref="IScopeManager.Active"/> is not null, and</item>
        /// <item>no <b>explicit</b> references are added via <see cref="AddReference"/>, and</item>
        /// <item><see cref="IgnoreActiveSpan"/> is not invoked,</item>
        /// </list>
        /// ... then an inferred <see cref="References.ChildOf"/> reference is created to the <see cref="IScopeManager.Active"/>
        /// <see cref="ISpanContext"/> when either <see cref="Start"/> or <see cref="StartActive"/> is invoked.
        /// </para>
        /// </summary>
        /// <param name="finishSpanOnDispose">
        /// Whether span should automatically be finished when <see cref="IDisposable.Dispose"/> is called.
        /// </param>
        /// <returns>An <see cref="IScope"/>, already registered via the <see cref="IScopeManager"/></returns>
        /// <seealso cref="IScopeManager"/>
        /// <seealso cref="IScope"/>
        IScope StartActive(bool finishSpanOnDispose);

        /// <summary>
        /// Like <see cref="StartActive"/>, but the returned <see cref="ISpan"/> has not been registered via the
        /// <see cref="IScopeManager"/>.
        /// </summary>
        /// <returns>
        /// The newly-started Span instance, which as *not* been automatically registered via the
        /// <see cref="IScopeManager"/>
        /// </returns>
        /// <seealso cref="StartActive(bool)"/>
        ISpan Start();
    }
}
