namespace OpenTracing
{
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
        /// <param name="operationName"></param>
        /// <returns>A new SpanBuilder.</returns>
        public static SpanBuilder BuildSpan(this ITracer tracer, string operationName)
        {
            return new SpanBuilder(tracer, operationName);
        }
    }
}
