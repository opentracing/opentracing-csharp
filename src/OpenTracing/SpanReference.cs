using System;

namespace OpenTracing
{
    /// <summary>
    /// A tuple used to represent a reference between two <see cref="ISpanContext"/>s.
    /// </summary>
    public class SpanReference
    {
        // TODO enum for types? (should the reference type be open?)

        /// <summary>
        /// Constant for the builtin reference type "child_of".
        /// </summary>
        public const string TypeChildOf = "child_of";

        /// <summary>
        /// Constant for the builtin reference type "follows_from".
        /// </summary>
        public const string TypeFollowsFrom = "follows_from";

        /// <summary>
        /// The type of the reference. "child_of" and "follows_from" are the builtin types.
        /// </summary>
        public string Type { get; }

        /// <summary>
        /// The referenced <see cref="ISpanContext"/>.
        /// </summary>
        public ISpanContext Context { get; }

        /// <summary>
        /// Creates a new reference to a SpanContext.
        /// </summary>
        /// <param name="referenceType">The type of the reference. "child_of" and "follows_from" are the builtin types.</param>
        /// <param name="context">The referenced <see cref="ISpanContext"/>.</param>
        public SpanReference(string referenceType, ISpanContext context)
        {
            if (string.IsNullOrWhiteSpace(referenceType))
            {
                throw new ArgumentNullException(nameof(referenceType));
            }

            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            Type = referenceType;
            Context = context;
        }

        /// <summary>
        /// Creates a new "child_of" reference for the <see cref="ISpanContext"/> of the given Span.
        /// </summary>
        /// <param name="parent">The parent Span.</param>
        public static SpanReference ChildOf(ISpan parent) => ChildOf(parent?.Context);

        /// <summary>
        /// Creates a new "child_of" reference.
        /// </summary>
        /// <param name="parent">The parent SpanContext.</param>
        public static SpanReference ChildOf(ISpanContext parent)
        {
            return parent != null ? new SpanReference(TypeChildOf, parent) : null;
        }

        /// <summary>
        /// Creates a new "follows_from" reference for the <see cref="ISpanContext"/> of the given Span.
        /// </summary>
        /// <param name="parent">The parent Span.</param>
        public static SpanReference FollowsFrom(ISpan parent) => FollowsFrom(parent?.Context);

        /// <summary>
        /// Creates a new "follows_from" reference.
        /// </summary>
        /// <param name="parent">The parent SpanContext.</param>
        public static SpanReference FollowsFrom(ISpanContext parent)
        {
            return parent != null ? new SpanReference(TypeFollowsFrom, parent) : null;
        }
    }
}