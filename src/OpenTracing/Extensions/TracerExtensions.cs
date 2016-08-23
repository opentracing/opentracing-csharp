using System;

namespace OpenTracing
{
    /// <summary>
    /// Extension methods <see cref="ITracer"/>.
    /// </summary>
    public static class TracerExtensions
    {
        /// <summary>
        /// Create, start, and return a new Span.
        /// </summary>
        /// <param name="tracer">A <see cref="ITracer"/> instance.</param>
        /// <param name="operationName">The operation name of the Span.</param>
        /// <param name="reference">A "child_of", "follows_from" or custom reference to another <see cref="ISpanContext"/>.</param>
        public static ISpan StartSpan(this ITracer tracer, string operationName, SpanReference reference)
        {
            if (tracer == null)
            {
                throw new ArgumentNullException(nameof(tracer));
            }

            var options = new StartSpanOptions()
                .AddReference(reference);

            return tracer.StartSpan(operationName, options);
        }

        /// <summary>
        /// Create, start, and return a new Span.
        /// </summary>
        /// <param name="tracer">An <see cref="ITracer"/> instance.</param>
        /// <param name="operationName">The operation name of the Span.</param>
        /// <param name="startTimestamp">The start timestamp that should be used for the new Span.</param>
        /// <param name="reference">A "child_of", "follows_from" or custom reference to another <see cref="ISpanContext"/>.</param>
        public static ISpan StartSpan(this ITracer tracer, string operationName, DateTimeOffset startTimestamp, SpanReference reference = null)
        {
            if (tracer == null)
            {
                throw new ArgumentNullException(nameof(tracer));
            }

            var options = new StartSpanOptions()
                .SetStartTimestamp(startTimestamp)
                .AddReference(reference);

            return tracer.StartSpan(operationName, options);
        }
    }
}