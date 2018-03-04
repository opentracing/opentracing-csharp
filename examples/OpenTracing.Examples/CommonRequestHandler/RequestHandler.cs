using System;
using System.Collections.Generic;
using OpenTracing.Tag;

namespace OpenTracing.Examples.CommonRequestHandler
{
    // One instance per Client. Executed concurrently for all requests of one client. 'beforeRequest'
    // and 'afterResponse' are executed in different threads for one 'send'
    public class RequestHandler
    {
        internal const String OperationName = "send";

        private readonly ITracer tracer;

        private readonly ISpanContext parentContext;

        public RequestHandler(ITracer tracer) : this(tracer, null)
        {
        }

        public RequestHandler(ITracer tracer, ISpanContext parentContext)
        {
            this.tracer = tracer;
            this.parentContext = parentContext;
        }

        public void BeforeRequest(Object request, Context context)
        {
            // we cannot use active span because we don't know in which thread it is executed
            // and we cannot therefore Activate span. thread can come from common thread pool.
            ISpanBuilder spanBuilder = tracer.BuildSpan(OperationName)
                    .IgnoreActiveSpan()
                    .WithTag(Tags.SpanKind.Key, Tags.SpanKindClient);

            if (parentContext != null)
            {
                spanBuilder.AsChildOf(parentContext);
            }

            context["span"] = spanBuilder.Start();
        }

        public void AfterResponse(Object response, Context context)
        {
            Object spanObject = context["span"];
            if (spanObject is ISpan)
            {
                ((ISpan)spanObject).Finish();
            }
        }
    }
}
