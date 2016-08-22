using System;

namespace OpenTracing
{
    public static class SpanExtensions
    {
        /// <summary>
        /// Convenience method to simplify logging.
        /// </summary>
        public static void Log(this ISpan span, string logEvent)
        {
            span.Log(new LogData(DateTime.Now, logEvent, null));
        }

        /// <summary>
        /// Convenience method to simplify logging.
        /// </summary>
        public static void Log(this ISpan span, string logEvent, object obj)
        {
            span.Log(new LogData(DateTime.Now, logEvent, obj));
        }
    }
}
