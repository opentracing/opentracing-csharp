using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;

namespace OpenTracing.AspNetCore
{
    public class OpenTracingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly OpenTracingOptions _options;

        public OpenTracingMiddleware(RequestDelegate next, OpenTracingOptions options)
        {
            if (next == null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _next = next;
            _options = options;
        }

        public Task Invoke(HttpContext context, ITracer tracer)
        {
            if (tracer == null)
            {
                throw new ArgumentNullException(nameof(tracer));
            }

            // see if this request is already part of a trace
            var existingSpanContext = tracer.ExtractFromHttpHeaders(context);
            ISpan requestSpan = null;

            try
            {
                // TODO peer tags?

                requestSpan = tracer.StartSpan(context.Request.Path, SpanReference.ChildOf(existingSpanContext))
                    .SetTagComponent("AspNetCore")
                    .SetTagSpanKindServer()
                    .SetTagHttpMethod(context.Request.Method)
                    .SetTagHttpUrl(UriHelper.GetDisplayUrl(context.Request))
                    .SetTag("TraceIdentifier", context.TraceIdentifier);

                // save span in http context. this allows subsequent components
                // to create child spans.
                context.Items.Add(typeof(ISpan), requestSpan);

                return _next(context);
            }
            finally
            {
                requestSpan?.SetTagHttpStatusCode(context.Response.StatusCode);
                requestSpan?.Finish(); // TODO only call Dispose() ?
                requestSpan?.Dispose();
            }
        }
    }
}