using System;

namespace OpenTracing.Propagation
{
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

        public bool Success { get; private set; }

        public Exception ExtractException { get; private set; }
    }
}
