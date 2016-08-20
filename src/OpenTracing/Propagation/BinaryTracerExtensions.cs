using System;
using OpenTracing.Propagation;

namespace OpenTracing
{
    public static class BinaryTracerExtensions
    {
        public static void InjectIntoBinary(this ITracer tracer, ISpanContext spanContext, byte[] carrier)
        {
            if (tracer == null)
            {
                throw new ArgumentNullException(nameof(tracer));
            }

            tracer.Inject(spanContext, new BinaryCarrier(carrier));
        }

        public static ISpanContext ExtractFromBinary(this ITracer tracer, byte[] carrier)
        {
            if (tracer == null)
            {
                throw new ArgumentNullException(nameof(tracer));
            }

            return tracer.Extract(new BinaryCarrier(carrier));
        }
    }
}