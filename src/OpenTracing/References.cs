namespace OpenTracing
{
    /// <summary>
    /// <para>References is essentially a namespace for the official OpenTracing reference types.</para>
    /// <para>References are used by <see cref="ISpanBuilder.AddReference"/> to describe the relationships between Spans.</para>
    /// </summary>
    public static class References
    {
        /// <summary>
        /// See http://opentracing.io/spec/#causal-span-references for more information about CHILD_OF references.
        /// </summary>
        public const string ChildOf = "child_of";

        /// <summary>
        /// See http://opentracing.io/spec/#causal-span-references for more information about FOLLOWS_FROM references.
        /// </summary>
        public const string FollowsFrom = "follows_from";
    }
}