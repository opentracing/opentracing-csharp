using System;

namespace OpenTracing
{
    public class ExtractResult
    {
        public ExtractResult(ISpan span)
        {
            Span = span;
            Success = true;
            ExtractException = null;
        }

        public ExtractResult(Exception e)
        {
            Span = null;
            Success = false;
            ExtractException = e;
        }

        public ISpan Span { get; private set; }

        public bool Success { get; private set; }

        public Exception ExtractException { get; private set; }
    }
}
