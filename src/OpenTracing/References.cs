namespace OpenTracing
{
    /// <summary>
    /// <para>References is essentially a namespace for the official OpenTracing reference types.</para>
    /// <para>References are used by <see cref="ISpanBuilder.AddReference"/> to describe the relationships between spans.</para>
    /// </summary>
    public static class References
    {
        /// <summary>
        /// A Span may be the ChildOf a parent Span. In a ChildOf reference, the parent Span depends on the child Span in some capacity
        /// <example>
        /// A Span representing the server side of an RPC may be the ChildOf a Span representing the client side of that RPC
        /// </example>
        /// <example>
        /// A Span representing a SQL insert may be the ChildOf a Span representing an ORM save method
        /// </example>
        /// <example>
        /// Many Spans doing concurrent (perhaps distributed) work may all individually be the ChildOf a single parent Span that merges the
        /// results for all children that return within a deadline
        /// </example>
        /// <seealso href="https://github.com/opentracing/specification/blob/master/specification.md#references-between-spans"/>
        /// </summary>
        public const string ChildOf = "child_of";

        /// <summary>
        /// Some parent Spans do not depend in any way on the result of their child Spans. In these cases, we say merely that the child Span
        /// FollowsFrom the parent Span in a causal sense. There are many distinct FollowsFrom reference sub-categories, and in future versions
        /// of OpenTracing they may be distinguished more formally.
        /// <seealso href="https://github.com/opentracing/specification/blob/master/specification.md#references-between-spans"/>
        /// </summary>
        public const string FollowsFrom = "follows_from";
    }
}