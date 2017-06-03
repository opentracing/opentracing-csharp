using System;

namespace OpenTracing
{
    /// <summary>
    /// <para>Every Span log has a specific timestamp and one or more fields.
    /// This class defines the standard fields.</para>
    /// <para>See https://github.com/opentracing/specification/blob/master/semantic_conventions.md for details.</para>
    /// </summary>
    public static class LogFields
    {
        /// <summary>
        /// The type or "kind" of an error (only for event="error" logs).
        /// <para />E.g., "Exception", "OSError".
        /// </summary>
        public const string ErrorKind = "error.kind";

        /// <summary>
        /// The actual <see cref="Exception"/> object instance.
        /// </summary>
        public const string ErrorObject = "error.object";

        /// <summary>
        /// A stable identifier for some notable moment in the lifetime of a Span.
        /// For instance, a mutex lock acquisition or release or the sorts of lifetime events
        /// in a browser page load described in the Performance.timing specification.
        /// <para />E.g., from Zipkin, "cs", "sr", "ss", or "cr". Or, more generally, "initialized" or "timed out". For errors, "error".
        /// </summary>
        public const string Event = "event";

        /// <summary>
        /// A concise, human-readable, one-line message explaining the event.
        /// <para />E.g., "Could not connect to backend", "Cache invalidation succeeded".
        /// </summary>
        public const string Message = "message";

        /// <summary>
        /// A stack trace in platform-conventional format; may or may not pertain to an error.
        /// <para />E.g.,
        /// <para />at App.Program.SomeMethod() in C:\app\Program.cs:line 14
        /// <para />at App.Program.Main(String[] args) in C:\app\Program.cs:line 9
        /// </summary>
        public const string Stack = "stack";
    }
}
