using System;
using System.Net.Http;
using System.Net.Http.Headers;
using OpenTracing.Propagation;

namespace OpenTracing
{
    public static class HttpHeadersTracerExtensions
    {
        public static void InjectIntoHttpHeaders(this ITracer tracer, ISpan span, HttpRequestMessage message)
        {
            InjectIntoHttpHeaders(tracer, span?.Context, message?.Headers);
        }

        public static void InjectIntoHttpHeaders(this ITracer tracer, ISpanContext spanContext, HttpRequestMessage message)
        {
            InjectIntoHttpHeaders(tracer, spanContext, message?.Headers);
        }

        public static void InjectIntoHttpHeaders(this ITracer tracer, ISpan span, HttpHeaders httpHeaders)
        {
            InjectIntoHttpHeaders(tracer, span?.Context, httpHeaders);
        }

        public static void InjectIntoHttpHeaders(this ITracer tracer, ISpanContext spanContext, HttpHeaders httpHeaders)
        {
            if (tracer == null)
            {
                throw new ArgumentNullException(nameof(tracer));
            }

            tracer.Inject(spanContext, new HttpHeadersCarrier(httpHeaders));
        }

        public static ISpanContext ExtractFromHttpHeaders(this ITracer tracer, HttpResponseMessage response)
        {
            // TODO is it useful to create a span from a http service-call?
            return ExtractFromHttpHeaders(tracer, response?.Headers);
        }

        public static ISpanContext ExtractFromHttpHeaders(this ITracer tracer, HttpHeaders headers)
        {
            if (tracer == null)
            {
                throw new ArgumentNullException(nameof(tracer));
            }

            return tracer.Extract(new HttpHeadersCarrier(headers));
        }
    }
}