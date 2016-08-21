using System;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace OpenTracing
{
    public static class HttpHeadersTracerExtensions
    {
        public static ISpanContext ExtractFromHttpHeaders(this ITracer tracer, HttpContext context)
        {
            return ExtractFromHttpHeaders(tracer, context?.Request?.Headers);
        }

        public static ISpanContext ExtractFromHttpHeaders(this ITracer tracer, HttpRequest request)
        {
            return ExtractFromHttpHeaders(tracer, request?.Headers);
        }

        public static ISpanContext ExtractFromHttpHeaders(this ITracer tracer, IHeaderDictionary headers)
        {
            if (tracer == null)
            {
                throw new ArgumentNullException(nameof(tracer));
            }

            if (headers == null)
            {
                throw new ArgumentNullException(nameof(headers));
            }

            // Map ASP.NET Core specific interface to more generic interface
            // which is needed by the HttpHeaders-carrier.
            var mappedHeaders = headers.ToDictionary(x => x.Key, x => x.Value.AsEnumerable());

            return tracer.ExtractFromHttpHeaders(mappedHeaders);
        }
    }
}