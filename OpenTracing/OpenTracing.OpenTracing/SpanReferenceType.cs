namespace OpenTracing
{
    public enum SpanReferenceType
    {
        // ChildOfRef refers to a parent Span that caused *and* somehow depends
        // upon the new child Span. Often (but not always), the parent Span cannot
        // finish unitl the child Span does.
        //
        // An timing diagram for a ChildOfRef that's blocked on the new Span:
        //
        //     [-Parent Span---------]
        //          [-Child Span----]
        //
        // See http://opentracing.io/spec/
        //
        // See opentracing.ChildOf()
        ChildOfRef = 1,

        // FollowsFromRef refers to a parent Span that does not depend in any way
        // on the result of the new child Span. For instance, one might use
        // FollowsFromRefs to describe pipeline stages separated by queues,
        // or a fire-and-forget cache insert at the tail end of a web request.
        //
        // A FollowsFromRef Span is part of the same logical trace as the new Span:
        // i.e., the new Span is somehow caused by the work of its FollowsFromRef.
        //
        // All of the following could be valid timing diagrams for children that
        // "FollowFrom" a parent.
        //
        //     [-Parent Span-]  [-Child Span-]
        //
        //
        //     [-Parent Span--]
        //      [-Child Span-]
        //
        //
        //     [-Parent Span-]
        //                 [-Child Span-]
        //
        // See http://opentracing.io/spec/
        //
        // See opentracing.FollowsFrom()
        FollowsFromRef,
    }
}
