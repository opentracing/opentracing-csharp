using System;

namespace OpenTracing
{
    /// <summary>
    /// Extension methods <see cref="ITracer"/>.
    /// </summary>
    public static class TracerExtensions
    {
        /// <summary>
        /// Convenience method to help build startoptions to create new span. 
        /// </summary>
        /// <example>
        /// <code>
        ///     ITracer tracer = ...
        /// 
        ///     ISpan parentSpan = tracer.BuildSpan("DoWork")
        ///                          .Start();
        /// 
        ///     Span http = tracer.BuildSpan("HandleHTTPRequest")
        ///                        .AsChildOf(parentSpan.context())
        ///                        .WithTag("user_agent", req.UserAgent)
        ///                        .WithTag("lucky_number", 42)
        ///                        .Start();
        /// </code>                       
        /// </example>
        /// <param name="tracer"> <see cref="ITracer"/> instance.</param>
        /// <param name="operationName">The operation name of the Span.</param>
        /// <returns>A new SpanBuilder.</returns>
        public static SpanBuilder BuildSpan(this ITracer tracer, string operationName)
        {
            return new SpanBuilder(tracer, operationName);
        }

        /// <summary>
        /// Create, start, and return a new Span.
        /// </summary>
        /// <param name="tracer">A <see cref="ITracer"/> instance.</param>
        /// <param name="operationName">The operation name of the Span.</param>
        /// <param name="reference">A "child_of", "follows_from" or custom reference to another <see cref="ISpanContext"/>.</param>
        public static ISpan StartSpan(this ITracer tracer, string operationName, SpanReference reference)
        {
            // TODO should we keep this extension method? (it would not create a builder-instance)

            if (tracer == null)
            {
                throw new ArgumentNullException(nameof(tracer));
            }

            StartSpanOptions options = null;
            if (reference != null)
            {
                options = new StartSpanOptions();
                options.References.Add(reference);
            }

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
            // TODO should we keep this extension method? (it would not create a builder-instance)

            if (tracer == null)
            {
                throw new ArgumentNullException(nameof(tracer));
            }

            var options = new StartSpanOptions { StartTimestamp = startTimestamp };
            if (reference != null)
            {
                options.References.Add(reference);
            }

            return tracer.StartSpan(operationName, options);
        }
    }
}