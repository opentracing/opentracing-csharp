using System;
using System.Collections.Generic;
using OpenTracing.Tag;

namespace OpenTracing.Examples.CommonRequestHandler
{
    // One instance per Client. Executed concurrently for all requests of one client. 'BeforeRequest'
    // and 'AfterResponse' may be executed in different threads for one 'send', but the active Span
    // will be properly propagated.
    public class RequestHandler
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

        public void BeforeRequest(Object request, Context context)
        {
            ISpanBuilder spanBuilder = _tracer.BuildSpan(OperationName)
                    .WithTag(Tags.SpanKind.Key, Tags.SpanKindClient);

            if (_ignoreActiveSpan)
            {
                spanBuilder.IgnoreActiveSpan();
            }

            context["span"] = spanBuilder.StartActive(true);
        }

        public void AfterResponse(Object response, Context context)
        {
            if (context["span"] is IScope scope)
            {
                scope.Dispose();
            }
        }
    }
}
