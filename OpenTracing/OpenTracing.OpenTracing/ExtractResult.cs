using System;

namespace OpenTracing
{
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

        public ISpanContext SpanContext { get; private set; }

        public bool Success { get; private set; }

        public Exception ExtractException { get; private set; }
    }
}
