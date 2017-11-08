using System;

namespace OpenTracing
{
    /// <summary>
    /// Builds a new <see cref="ISpan" />.
    /// </summary>
    public interface ISpanBuilder
    {
        /// <summary>
        /// Sets the operation name for the given span
        /// </summary>
        /// <param name="operationName">
        /// An operation name, a human-readable string which concisely represents the work done by the Span (for example, an RPC method name, 
        /// a function name, or the name of a subtask or stage within a larger computation). The operation name should be the most general 
        /// string that identifies a (statistically) interesting class of Span instances. That is, "get_user" is better than "get_user/314159".
        /// <example>
        /// <c>get</c> Too general
        /// <c>get_account/792</c> Too specific
        /// <c>get_account</c> Good, and <c>account_id=792</c> would make a nice <see cref="ISpan"/> Tag.
        /// </example>
        /// </param>
        ISpanBuilder SetOperationName(string operationName);

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
        /// <returns>The current <see cref="ISpanBuilder"/> instance, for chaining.</returns>
        ISpanBuilder FollowsFrom(ISpan parent);

        /// <summary>
        /// A shorthand for <see cref="AddReference(string, ISpanContext)" /> using <see cref="References.FollowsFrom"/>.
        /// </summary>
        /// <returns>The current <see cref="ISpanBuilder"/> instance, for chaining.</returns>
        ISpanBuilder FollowsFrom(ISpanContext parent);

        /// <summary>
        /// Add a reference from the span being built to a distinct (usually parent) span.
        /// May be called multiple times to represent multiple such references.
        /// </summary>
        /// <param name="referenceType">The reference type, typically one of the constants defined in <see cref="References"/>.</param>
        /// <param name="referencedContext">The <see cref="ISpanContext"/> being referenced;
        /// e.g., for a <see cref="References.ChildOf"/> referenceType, the referencedContext is the parent.</param>
        /// <returns>The current <see cref="ISpanBuilder"/> instance, for chaining.</returns>
        /// <seealso cref="FollowsFrom(OpenTracing.ISpan)"/>
        /// <seealso cref="AsChildOf(OpenTracing.ISpan)"/>
        ISpanBuilder AddReference(string referenceType, ISpanContext referencedContext);

        /// <summary>
        /// Adds a tag to the span.
        /// </summary>
        /// <param name="key">If there is a pre-existing tag set for <paramref name="key"/>, it is overwritten.</param>
        /// <param name="value">The value to be stored.</param>
        /// <returns>The current <see cref="ISpanBuilder"/> instance, for chaining.</returns>
        /// <seealso cref="Tags"/>
        ISpanBuilder SetTag(string key, bool value);

        /// <summary>
        /// Adds a tag to the span.
        /// </summary>
        /// <param name="key">If there is a pre-existing tag set for <paramref name="key"/>, it is overwritten.</param>
        /// <param name="value">The value to be stored.</param>
        /// <returns>The current <see cref="ISpanBuilder"/> instance, for chaining.</returns>
        /// <seealso cref="Tags"/>
        ISpanBuilder SetTag(string key, double value);

        /// <summary>
        /// Adds a tag to the span.
        /// </summary>
        /// <param name="key">If there is a pre-existing tag set for <paramref name="key"/>, it is overwritten.</param>
        /// <param name="value">The value to be stored.</param>
        /// <returns>The current <see cref="ISpan"/> instance for chaining.</returns>
        /// <seealso cref="Tags"/>
        ISpanBuilder SetTag(string key, int value);

        /// <summary>
        /// Adds a tag to the span.
        /// </summary>
        /// <param name="key">If there is a pre-existing tag set for <paramref name="key"/>, it is overwritten.</param>
        /// <param name="value">The value to be stored.</param>
        /// <returns>The current <see cref="ISpan"/> instance for chaining.</returns>
        /// <seealso cref="Tags"/>
        ISpanBuilder SetTag(string key, string value);

        /// <summary>
        /// Specify an explicit timestamp of when the span was started.
        /// If omitted, the current walltime (as of <see cref="Start"/>) is used by default.
        /// </summary>
        /// <param name="startTimestamp">An explicit start timestamp for the span.</param>
        ISpanBuilder SetStartTimestamp(DateTimeOffset startTimestamp);

        /// <summary>
        /// Returns the started <see cref="ISpan"/>.
        /// </summary>
        ISpan Start();
    }
}