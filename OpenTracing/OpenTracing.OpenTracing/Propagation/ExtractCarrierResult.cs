using System;

namespace OpenTracing.Propagation
{
    public class ExtractCarrierResult<T>
    {
        public ExtractCarrierResult(T formatData)
        {
            FormatData = formatData;
            Success = true;
            ExtractException = null;
        }

        public ExtractCarrierResult(Exception e)
        {
            FormatData = default(T);
            Success = false;
            ExtractException = e;
        }

        public T FormatData { get; private set; }

        public bool Success { get; private set; }

        public Exception ExtractException { get; private set; }
    }
}
