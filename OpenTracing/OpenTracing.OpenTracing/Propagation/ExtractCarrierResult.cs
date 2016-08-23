using System;

namespace OpenTracing.Propagation
{
    /// <summary>
    /// Represents the results of a carrier.Extract call.
    /// </summary>
    /// <typeparam name="TFormat"></typeparam>
    public class ExtractCarrierResult<TFormat>
    {
        public ExtractCarrierResult(TFormat formatData)
        {
            FormatData = formatData;
            Success = true;
            ExtractException = null;
        }

        public ExtractCarrierResult(Exception e)
        {
            FormatData = default(TFormat);
            Success = false;
            ExtractException = e;
        }

        public TFormat FormatData { get; private set; }

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
