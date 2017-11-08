namespace OpenTracing
{
    /// <summary>
    /// Every Span log has a specific timestamp (which must fall between the start and finish timestamps of the Span, inclusive)
    /// and one or more fields
    /// <seealso href="https://github.com/opentracing/specification/blob/master/semantic_conventions.md"/>
    /// </summary>
    public static class StandardLogTags
    {
        public static class Error
        {
            /// <summary>
            /// The type or "kind" of an error (only for event="error" logs). E.g., "Exception", "OSError"
            /// </summary>
            public const string Kind = "error.kind";

            /// <summary>
            /// For languages that support such a thing (e.g., Java, Python, C# :D), the actual Throwable/Exception/Error object 
            /// instance itself. E.g., A java.lang.UnsupportedOperationException instance, a python exceptions.NameError instance
            /// </summary>
            /// <remarks>
            /// It is possible taht the value came from another language system - implementors should not presume the contents
            /// hold a <see cref="System.Exception"/>.
            /// </remarks>
            public const string Object = "error.object";
        }

        /// <summary>
        /// A stable identifier for some notable moment in the lifetime of a Span. For instance, a mutex lock acquisition
        /// or release or the sorts of lifetime events in a browser page load described in the Performance.timing
        /// specification. E.g., from Zipkin, "cs", "sr", "ss", or "cr". Or, more generally, "initialized" or "timed out".
        /// For errors, "error"
        /// </summary>
        public const string Event = "event";

        /// <summary>
        /// A concise, human-readable, one-line message explaining the event. E.g., "Could not connect to backend", 
        /// "Cache invalidation succeeded"
        /// </summary>
        public const string Message = "message";

        /// <summary>
        /// A stack trace in platform-conventional format; may or may not pertain to an error. E.g., "File \"example.py\", 
        /// line 7, in \<module\>\ncaller()\nFile \"example.py\", line 5, in caller\ncallee()\nFile \"example.py\", 
        /// line 2, in callee\nraise Exception(\"Yikes\")\n"
        /// </summary>
        public const string Stack = "stack";
    }
}