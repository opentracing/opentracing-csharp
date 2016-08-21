using System;

namespace OpenTracing
{
    public static class SpanExtensions
    {
        public static ISpan IsChildOf(this ISpan span, ISpan parent)
        {
            return IsChildOf(span, parent?.Context);
        }

        public static ISpan IsChildOf(this ISpan span, ISpanContext parent)
        {
            if (span == null)
            {
                throw new ArgumentNullException(nameof(span));
            }

            return span.AddReference(ReferenceTypes.ChildOf, parent);
        }

        public static ISpan FollowsFrom(this ISpan span, ISpan parent)
        {
            return FollowsFrom(span, parent?.Context);
        }

        public static ISpan FollowsFrom(this ISpan span, ISpanContext parent)
        {
            if (span == null)
            {
                throw new ArgumentNullException(nameof(span));
            }

            return span.AddReference(ReferenceTypes.FollowsFrom, parent);
        }
    }
}