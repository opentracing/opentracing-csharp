using System;

namespace OpenTracing
{
    /// <summary>
    /// Builds a new <see cref="ISpan" />.
    /// </summary>
    public interface ISpanBuilder
    {
        /// <summary>
        /// A shorthand for <see cref="AddReference(SpanReference.TypeChildOf, parent.Context)" />.
        /// </summary>
        ISpanBuilder AsChildOf(ISpan parent);

        /// <summary>
        /// A shorthand for <see cref="AddReference(SpanReference.TypeChildOf, parent)" />.
        /// </summary>
        ISpanBuilder AsChildOf(ISpanContext parent);

        /// <summary>
        /// A shorthand for <see cref="AddReference(SpanReference.TypeFollowsFrom, parent.Context)" />.
        /// </summary>
        ISpanBuilder FollowsFrom(ISpan parent);

        /// <summary>
        /// A shorthand for <see cref="AddReference(SpanReference.TypeFollowsFrom, parent)" />.
        /// </summary>
        ISpanBuilder FollowsFrom(ISpanContext parent);

        /// <summary>
        /// Add a reference from the Span being built to a distinct (usually parent) Span.
        /// May be called multiple times to represent multiple such references.
        /// If the <paramref name="reference"/> is null, the option has no effect.
        /// </summary>
        /// <param name="reference">The reference that should be added to the new Span.</param>
        ISpanBuilder AddReference(SpanReference reference);

        /// <summary>
        /// Same as <see cref="ISpan.SetTag" />, but for the span being built.
        /// </summary>
        ISpanBuilder WithTag(string key, object value);

        /// <summary>
        /// Specify a timestamp of when the Span was started.
        /// </summary>
        /// <param name="startTimestamp">
        ///   The timestamp of when the Span was started.
        ///   Use <see cref="DateTimeKind.Utc"/> whenever possible. The behavior of other kinds is not defined.
        /// </param>
        ISpanBuilder WithStartTimestamp(DateTime startTimestamp);

        /// <summary>
        /// Returns the started Span.
        /// </summary>
        ISpan Start();
    }
}