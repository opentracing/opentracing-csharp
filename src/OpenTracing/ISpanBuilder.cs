using System;

namespace OpenTracing
{
    /// <summary>
    /// Builds a new <see cref="ISpan" />.
    /// </summary>
    public interface ISpanBuilder
    {
        /// <summary>
        /// A shorthand for <see cref="AddReference(References.ChildOf, parent.Context)" />.
        /// </summary>
        ISpanBuilder AsChildOf(ISpan parent);

        /// <summary>
        /// A shorthand for <see cref="AddReference(References.ChildOf, parent)" />.
        /// </summary>
        ISpanBuilder AsChildOf(ISpanContext parent);

        /// <summary>
        /// A shorthand for <see cref="AddReference(References.FollowsFrom, parent.Context)" />.
        /// </summary>
        ISpanBuilder FollowsFrom(ISpan parent);

        /// <summary>
        /// A shorthand for <see cref="AddReference(References.FollowsFrom, parent)" />.
        /// </summary>
        ISpanBuilder FollowsFrom(ISpanContext parent);

        /// <summary>
        /// Add a reference from the Span being built to a distinct (usually parent) Span.
        /// May be called multiple times to represent multiple such references.
        /// </summary>
        /// <param name="referenceType">The reference type, typically one of the constants defined in References.</param>
        /// <param name="referencedContext">The <see cref="ISpanContext"/> being referenced;
        /// e.g., for a References.ChildOf referenceType, the referencedContext is the parent.</param>
        ISpanBuilder AddReference(string referenceType, ISpanContext referencedContext);

        /// <summary>
        /// Adds a tag to the Span.
        /// </summary>
        /// <param name="key">If there is a pre-existing tag set for <paramref name="key"/>, it is overwritten.</param>
        /// <param name="value">The value to be stored.</param>
        /// <returns>The current <see cref="ISpan"/> instance for chaining.</returns>
        ISpanBuilder WithTag(string key, bool value);

        /// <summary>
        /// Adds a tag to the Span.
        /// </summary>
        /// <param name="key">If there is a pre-existing tag set for <paramref name="key"/>, it is overwritten.</param>
        /// <param name="value">The value to be stored.</param>
        /// <returns>The current <see cref="ISpan"/> instance for chaining.</returns>
        ISpanBuilder WithTag(string key, double value);

        /// <summary>
        /// Adds a tag to the Span.
        /// </summary>
        /// <param name="key">If there is a pre-existing tag set for <paramref name="key"/>, it is overwritten.</param>
        /// <param name="value">The value to be stored.</param>
        /// <returns>The current <see cref="ISpan"/> instance for chaining.</returns>
        ISpanBuilder WithTag(string key, string value);

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