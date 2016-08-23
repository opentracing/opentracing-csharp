using System;
using Microsoft.AspNetCore.Http;
using OpenTracing.AspNetCore;
using OpenTracing.Propagation;

namespace OpenTracing
{
    public static class AspNetCoreFormatExtensions
    {
        public static ISpanContext ExtractHttpHeaders(this ITracer tracer, HttpContext context)
        {
            return ExtractHttpHeaders(tracer, context?.Request?.Headers);
        }

        public static ISpanContext ExtractHttpHeaders(this ITracer tracer, HttpRequest request)
        {
            return ExtractHttpHeaders(tracer, request?.Headers);
        }

        public static ISpanContext ExtractHttpHeaders(this ITracer tracer, IHeaderDictionary headers)
        {
            if (tracer == null)
            {
                throw new ArgumentNullException(nameof(tracer));
            }

            if (headers == null)
            {
                throw new ArgumentNullException(nameof(headers));
            }

            return tracer.Extract(Formats.HttpHeaders, new HeaderDictionaryCarrier(headers));
        }
    }
}