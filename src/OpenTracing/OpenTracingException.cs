using System;

namespace OpenTracing
{
    /// <summary>This is the base type for all OpenTracing specific exceptions.</summary>
    public abstract class OpenTracingException : Exception
    {
        protected OpenTracingException()
        {
        }

        protected OpenTracingException(string message) : base(message)
        {
        }

        protected OpenTracingException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
