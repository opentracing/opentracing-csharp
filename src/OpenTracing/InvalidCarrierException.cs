using System;

namespace OpenTracing
{
    /// <summary>
    /// This exception should be used when the provided carrier instance does not match what the "format" argument requires.
    /// </summary>
    public class InvalidCarrierException : Exception
    {
        public InvalidCarrierException()
        {
        }

        public InvalidCarrierException(string message) : base(message)
        {
        }

        public InvalidCarrierException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}