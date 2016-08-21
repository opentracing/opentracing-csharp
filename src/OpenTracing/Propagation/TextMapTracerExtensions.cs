using System;
using System.Collections.Generic;
using OpenTracing.Propagation;

namespace OpenTracing
{
    public static class TextMapTracerExtensions
    {
        public static void InjectIntoTextMap(this ITracer tracer, ISpan span, IDictionary<string, string> carrier)
        {
            InjectIntoTextMap(tracer, span?.Context, carrier);
        }

        public static void InjectIntoTextMap(this ITracer tracer, ISpanContext spanContext, IDictionary<string, string> carrier)
        {
            if (tracer == null)
            {
                throw new ArgumentNullException(nameof(tracer));
            }

            tracer.Inject(spanContext, new TextMapCarrier(carrier));
        }

        public static ISpanContext ExtractFromTextMap(this ITracer tracer, IDictionary<string, string> carrier)
        {
            if (tracer == null)
            {
                throw new ArgumentNullException(nameof(tracer));
            }

            return tracer.Extract(new TextMapCarrier(carrier));
        }
    }
}