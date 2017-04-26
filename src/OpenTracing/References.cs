namespace OpenTracing
{
    /// <summary>
    /// <para>References is essentially a namespace for the official OpenTracing reference types.</para>
    /// <para>References are used by <see cref="ISpanBuilder.AddReference"/> to describe the relationships between spans.</para>
    /// </summary>
    public static class References
    {
        /// <summary>
        /// See https://github.com/opentracing/specification/blob/master/specification.md#references-between-spans
        /// for more information about CHILD_OF references.
        /// </summary>
        public const string ChildOf = "child_of";

        /// <summary>
        /// See https://github.com/opentracing/specification/blob/master/specification.md#references-between-spans
        /// for more information about FOLLOWS_FROM references.
        /// </summary>
        public const string FollowsFrom = "follows_from";
    }
}