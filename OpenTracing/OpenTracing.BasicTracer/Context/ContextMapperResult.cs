using System;

namespace OpenTracing.BasicTracer.Context
{
    public class ContextMapToResult<TContext>
    {
        public ContextMapToResult(TContext spanContext)
        {
            SpanContext = spanContext;
            Success = true;
            MapException = null;
        }

        public ContextMapToResult(Exception mapException)
        {
            SpanContext = default(TContext);
            Success = false;
            MapException = mapException;
        }

        public TContext SpanContext { get; private set; }

        public bool Success { get; private set; }

        public Exception MapException { get; private set; }
    }
}
