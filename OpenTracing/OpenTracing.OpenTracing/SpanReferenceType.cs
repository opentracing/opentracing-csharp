namespace OpenTracing
{
    /// <summary>
    /// SpanReferenceType is an enum type describing different categories of
    /// relationships between two Spans. If Span-2 refers to Span-1, the
    /// SpanReferenceType describes Span-1 from Span-2's perspective. For example,
    /// ChildOfRef means that Span-1 created Span-2.
    ///
    /// NOTE: Span-1 and Span-2 do *not* necessarily depend on each other for
    /// completion; e.g., Span-2 may be part of a background job enqueued by Span-1,
    /// or Span-2 may be sitting in a distributed queue behind Span-1.
    /// </summary>
    public enum SpanReferenceType
    {
        /// <summary>
        /// ChildOfRef refers to a parent Span that caused *and* somehow depends
        /// upon the new child Span. Often (but not always), the parent Span cannot
        /// finish unitl the child Span does.
        ///
        /// An timing diagram for a ChildOfRef that's blocked on the new Span:
        ///
        ///     [-Parent Span---------]
        ///          [-Child Span----]
        ///
        /// See http://opentracing.io/spec/
        /// </summary>
        ChildOfRef = 1,

        /// <summary>
        /// FollowsFromRef refers to a parent Span that does not depend in any way
        /// on the result of the new child Span. For instance, one might use
        /// FollowsFromRefs to describe pipeline stages separated by queues,
        /// or a fire-and-forget cache insert at the tail end of a web request.
        ///
        /// A FollowsFromRef Span is part of the same logical trace as the new Span:
        /// i.e., the new Span is somehow caused by the work of its FollowsFromRef.
        ///
        /// All of the following could be valid timing diagrams for children that
        /// "FollowFrom" a parent.
        ///
        ///     [-Parent Span-]  [-Child Span-]
        ///
        ///
        ///     [-Parent Span--]
        ///      [-Child Span-]
        ///
        ///
        ///     [-Parent Span-]
        ///                 [-Child Span-]
        ///
        /// See http://opentracing.io/spec/
        /// </summary>
        FollowsFromRef,
    }
}
