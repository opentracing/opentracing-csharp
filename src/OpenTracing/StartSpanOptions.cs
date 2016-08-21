using System;
using System.Collections.Generic;

namespace OpenTracing
{
    public class StartSpanOptions
    {
        public IList<SpanReference> References { get; } = new List<SpanReference>();

        public DateTimeOffset? StartTimestamp { get; set; }

        #region Fluent Interface

        public StartSpanOptions ChildOf(ISpan parent)
        {
            return AddReference(SpanReference.ChildOf(parent));
        }

        public StartSpanOptions ChildOf(ISpanContext parent)
        {
            return AddReference(SpanReference.ChildOf(parent));
        }

        public StartSpanOptions FollowsFrom(ISpan parent)
        {
            return AddReference(SpanReference.FollowsFrom(parent));
        }

        public StartSpanOptions FollowsFrom(ISpanContext parent)
        {
            return AddReference(SpanReference.FollowsFrom(parent));
        }

        public StartSpanOptions AddReference(SpanReference reference)
        {
            if (reference != null)
            {
                References.Add(reference);
            }

            return this;
        }

        public StartSpanOptions SetStartTimestamp(DateTimeOffset startTimestamp)
        {
            StartTimestamp = startTimestamp;
            return this;
        }

        #endregion Fluent Interface
    }
}