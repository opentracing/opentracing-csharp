namespace OpenTracing
{
    /// <summary>
    /// The following log fields are recommended for instrumentors who are trying to capture more information about a
    /// logged event. Tracers may expose additional features based on these standardized data points.
    /// <see href="https://github.com/opentracing/specification/blob/master/semantic_conventions.md"/>
    /// </summary>
    public static class LogFields
    {
        /// <summary>The type or "kind" of an error (only for event="error" logs). E.g., "Exception", "OSError".</summary>
        public const string ErrorKind = "error.kind";

        /// <summary>
        /// The actual <see cref="System.Exception"/> object instance.
        /// </summary>
        public const string ErrorObject = "error.object";

        /// <summary>
        /// A stable identifier for some notable moment in the lifetime of a span. For instance, a mutex lock acquisition
        /// or release or the sorts of lifetime events in a browser page load described in the Performance.timing specification.
        /// E.g., from Zipkin, "cs", "sr", "ss", or "cr". Or, more generally, "initialized" or "timed out". For errors, "error".
        /// </summary>
        public const string Event = "event";

        /// <summary>
        /// A concise, human-readable, one-line message explaining the event. E.g., "Could not connect to backend", "Cache
        /// invalidation succeeded".
        /// </summary>
        public const string Message = "message";

        /// <summary>A stack trace in platform-conventional format; may or may not pertain to an error.</summary>
        /// <example>
        /// <code>
        /// at App.Program.SomeMethod() in C:\app\Program.cs:line 14
        /// at App.Program.Main(String[] args) in C:\app\Program.cs:line 9
        /// </code>
        /// </example>
        public const string Stack = "stack";
    }
}
