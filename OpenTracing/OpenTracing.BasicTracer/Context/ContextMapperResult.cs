using System;

namespace OpenTracing.BasicTracer.Context
{
    public class ContextMapToResult<T>
    {
        public ContextMapToResult(T spanContext)
        {
            SpanContext = spanContext;
            Success = true;
            MapException = null;
        }

        public ContextMapToResult(Exception mapException)
        {
            SpanContext = default(T);
            Success = false;
            MapException = mapException;
        }

        public T SpanContext { get; private set; }

        public bool Success { get; private set; }

        public Exception MapException { get; private set; }
    }
}
