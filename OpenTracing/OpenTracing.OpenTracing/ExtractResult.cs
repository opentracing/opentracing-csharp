using System;

namespace OpenTracing
{
    /// <summary>
    /// Represents the results of a Tracer.Extract call.
    /// </summary>
    public class ExtractResult
    {
        public ExtractResult(ISpanContext spanContext)
        {
            SpanContext = spanContext;
            Success = true;
            ExtractException = null;
        }

        public ExtractResult(Exception e)
        {
            SpanContext = null;
            Success = false;
            ExtractException = e;
        }

        /// <summary>
        /// If extraction was successful this contains an ISpanContext instance holding 
        /// the extracted context.
        /// If extraction was unsuccessful, then SpanContext == null
        /// </summary>
        public ISpanContext SpanContext { get; private set; }

        /// <summary>
        /// True if the extraction was successful
        /// </summary>
        public bool Success { get; private set; }

        /// <summary>
        /// If extraction was unsuccessful and an exception has been caught internally, 
        /// this field will contain the exception.
        /// </summary>
        public Exception ExtractException { get; private set; }
    }
}
