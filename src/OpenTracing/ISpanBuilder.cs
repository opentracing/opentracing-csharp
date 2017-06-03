using System;

namespace OpenTracing
{
    /// <summary>
    /// Builds a new <see cref="ISpan" />.
    /// </summary>
    public interface ISpanBuilder
    {
        /// <summary>
        /// A shorthand for <see cref="AddReference(string, ISpanContext)" /> using <see cref="References.ChildOf"/>.
        /// </summary>
        ISpanBuilder AsChildOf(ISpan parent);

        /// <summary>
        /// A shorthand for <see cref="AddReference(string, ISpanContext)" /> using <see cref="References.ChildOf"/>.
        /// </summary>
        ISpanBuilder AsChildOf(ISpanContext parent);

        /// <summary>
        /// A shorthand for <see cref="AddReference(string, ISpanContext)" /> using <see cref="References.FollowsFrom"/>.
        /// </summary>
        ISpanBuilder FollowsFrom(ISpan parent);

        /// <summary>
        /// A shorthand for <see cref="AddReference(string, ISpanContext)" /> using <see cref="References.FollowsFrom"/>.
        /// </summary>
        ISpanBuilder FollowsFrom(ISpanContext parent);

        /// <summary>
        /// Add a reference from the span being built to a distinct (usually parent) span.
        /// May be called multiple times to represent multiple such references.
        /// </summary>
        /// <param name="referenceType">The reference type, typically one of the constants defined in <see cref="References"/>.</param>
        /// <param name="referencedContext">The <see cref="ISpanContext"/> being referenced;
        /// e.g., for a <see cref="References.ChildOf"/> referenceType, the referencedContext is the parent.</param>
        ISpanBuilder AddReference(string referenceType, ISpanContext referencedContext);

        /// <summary>
        /// When used, the newly created span will NOT inherit <see cref="ITracer.ActiveSpan"/> as a parent.
        /// </summary>
        ISpanBuilder IgnoreActiveSpan();

        /// <summary>
        /// Adds a tag to the span.
        /// </summary>
        /// <param name="key">If there is a pre-existing tag set for <paramref name="key"/>, it is overwritten.</param>
        /// <param name="value">The value to be stored.</param>
        /// <returns>The current <see cref="ISpan"/> instance for chaining.</returns>
        ISpanBuilder WithTag(string key, bool value);

        /// <summary>
        /// Adds a tag to the span.
        /// </summary>
        /// <param name="key">If there is a pre-existing tag set for <paramref name="key"/>, it is overwritten.</param>
        /// <param name="value">The value to be stored.</param>
        /// <returns>The current <see cref="ISpan"/> instance for chaining.</returns>
        ISpanBuilder WithTag(string key, double value);

        /// <summary>
        /// Adds a tag to the span.
        /// </summary>
        /// <param name="key">If there is a pre-existing tag set for <paramref name="key"/>, it is overwritten.</param>
        /// <param name="value">The value to be stored.</param>
        /// <returns>The current <see cref="ISpan"/> instance for chaining.</returns>
        ISpanBuilder WithTag(string key, int value);

        /// <summary>
        /// Adds a tag to the span.
        /// </summary>
        /// <param name="key">If there is a pre-existing tag set for <paramref name="key"/>, it is overwritten.</param>
        /// <param name="value">The value to be stored.</param>
        /// <returns>The current <see cref="ISpan"/> instance for chaining.</returns>
        ISpanBuilder WithTag(string key, string value);

        /// <summary>
        /// Specify a timestamp of when the span was started.
        /// </summary>
        /// <param name="startTimestamp">
        /// An explicit start timestamp for the span.
        /// The behavior for kinds other than <see cref="DateTimeKind.Utc"/> is undefined.
        /// </param>
        ISpanBuilder WithStartTimestamp(DateTime startTimestamp);

        /// <summary>
        /// Returns the started span.
        /// </summary>
        ISpan Start();
    }
}