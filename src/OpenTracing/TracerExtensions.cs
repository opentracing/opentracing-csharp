using System;

namespace OpenTracing
{
    public static class TracerExtensions
    {
        
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