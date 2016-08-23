using System;

namespace OpenTracing
{
    public static class SpanExtensions
    {
        /// <summary>
        /// Adds logdata to the span.
        /// </summary>
        public static void Log(this ISpan span, string logEvent)
        {
            span.Log(new LogData(DateTime.Now, logEvent, null));
        }

        /// <summary>
        /// Adds logdata to the span.
        /// </summary>
        public static void Log(this ISpan span, string logEvent, object payload)
        {
            span.Log(new LogData(DateTime.Now, logEvent, payload));
        }
    }
}
