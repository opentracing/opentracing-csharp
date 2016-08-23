using System;
using System.Collections.Generic;

namespace OpenTracing
{
    /// <summary>
    /// Provides a way to configure the behavior of <see cref="ITracer.StartSpan"/>.
    /// </summary>
    public class StartSpanOptions
    {
        /// <summary>
        /// A list of references to other <see cref="ISpanContext"/>s.
        /// </summary>
        public IList<SpanReference> References { get; } = new List<SpanReference>();

        /// <summary>
        /// The start timestamp that should be used for the new Span.
        /// </summary>
        public DateTimeOffset? StartTimestamp { get; set; }

        #region Fluent Interface

        /// <summary>
        /// Marks the new Span as a child of the given <paramref name="parent"/>.
        /// </summary>
        /// <param name="parent">The parent span.</param>
        /// <returns>The current instance for chaining.</returns>
        public StartSpanOptions ChildOf(ISpan parent)
        {
            return AddReference(SpanReference.ChildOf(parent));
        }

        /// <summary>
        /// Marks the new Span as a child of the given <paramref name="parent"/>.
        /// </summary>
        /// <param name="parent">The parent span.</param>
        /// <returns>The current instance for chaining.</returns>
        public StartSpanOptions ChildOf(ISpanContext parent)
        {
            return AddReference(SpanReference.ChildOf(parent));
        }

        /// <summary>
        /// Marks the new Span as a follower from the given <paramref name="parent"/>.
        /// </summary>
        /// <param name="parent">The parent span.</param>
        /// <returns>The current instance for chaining.</returns>
        public StartSpanOptions FollowsFrom(ISpan parent)
        {
            return AddReference(SpanReference.FollowsFrom(parent));
        }

        /// <summary>
        /// Marks the new Span as a follower from the given <paramref name="parent"/>.
        /// </summary>
        /// <param name="parent">The parent span.</param>
        /// <returns>The current instance for chaining.</returns>
        public StartSpanOptions FollowsFrom(ISpanContext parent)
        {
            return AddReference(SpanReference.FollowsFrom(parent));
        }

        /// <summary>
        /// Adds a reference to the new Span.
        /// </summary>
        /// <param name="reference">The reference that should be added to the new Span.</param>
        /// <returns>The current instance for chaining.</returns>
        public StartSpanOptions AddReference(SpanReference reference)
        {
            if (reference != null)
            {
                References.Add(reference);
            }

            return this;
        }

        /// <summary>
        /// The start timestamp that should be used for the new Span.
        /// </summary>
        /// <param name="startTimestamp">The start timestamp that should be used for the new Span.</param>
        /// <returns>The current instance for chaining.</returns>
        public StartSpanOptions SetStartTimestamp(DateTimeOffset startTimestamp)
        {
            StartTimestamp = startTimestamp;
            return this;
        }

        #endregion Fluent Interface
    }
}