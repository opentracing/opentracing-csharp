namespace OpenTracing
{
    using System;

    /// <summary>
    /// This exception should be used when the underlying span context state is seemingly present but not well-formed.
    /// </summary>
    public class SpanContextCorruptedException : OpenTracingException
    {
        public SpanContextCorruptedException()
        {
        }

        public SpanContextCorruptedException(string message) : base(message)
        {
        }

        public SpanContextCorruptedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}