using System;

namespace OpenTracing.Propagation
{
    /// <summary>
    /// Represents the results of a carrier.Extract call.
    /// </summary>
    /// <typeparam name="TFormat"></typeparam>
    public class ExtractCarrierResult<TFormat>
    {
        public ExtractCarrierResult(TFormat context)
        {
            Context = context;
            Success = true;
            ExtractException = null;
        }

        public ExtractCarrierResult(Exception e)
        {
            Context = default(TFormat);
            Success = false;
            ExtractException = e;
        }

        /// <summary>
        /// If extraction was successful this contains the extracted context data 
        /// in the TFormat type.

        /// If extraction was unsuccessful, then SpanContext == null
        /// </summary>
        public TFormat Context { get; private set; }

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
