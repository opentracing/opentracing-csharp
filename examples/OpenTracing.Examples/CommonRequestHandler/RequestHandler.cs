using System;
using System.Collections.Generic;
using OpenTracing.Tag;

namespace OpenTracing.Examples.CommonRequestHandler
{
    public interface IRequestHandler
    {
        void BeforeRequest(object request, Context context);
        void AfterResponse(object response, Context context);
    }

    // One instance per Client. Executed concurrently for all requests of one client. 'BeforeRequest'
    // and 'AfterResponse' may be executed in different threads for one 'send', but the active Span
    // will be properly propagated.
    public class RequestHandler : IRequestHandler
    {
        internal const string OperationName = "send";

        private readonly ITracer _tracer;

        private readonly bool _ignoreActiveSpan;

        public RequestHandler(ITracer tracer) : this(tracer, false)
        {
        }

        public RequestHandler(ITracer tracer, bool ignoreActiveSpan)
        {
            _tracer = tracer;
            _ignoreActiveSpan = ignoreActiveSpan;
        }

        public void BeforeRequest(object request, Context context)
        {
            ISpanBuilder spanBuilder = _tracer.BuildSpan(OperationName)
                    .WithTag(Tags.SpanKind.Key, Tags.SpanKindClient);

            if (_ignoreActiveSpan)
            {
                spanBuilder.IgnoreActiveSpan();
            }

            // No need to put 'span' in Context, as our ScopeManager
            // will automatically propagate it, even when switching between threads,
            // and will be available when AfterResponse() is called.
            spanBuilder.StartActive(true);
        }

        public void AfterResponse(object response, Context context)
        {
            _tracer.ScopeManager.Active.Dispose();
        }
    }
}
