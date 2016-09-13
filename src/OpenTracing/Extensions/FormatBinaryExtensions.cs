using System;
using OpenTracing.Propagation;

namespace OpenTracing
{
    /// <summary>
    /// Contains <see cref="ITracer.Inject"/> and <see cref="ITracer.Extract"/> extension methods for types 
    /// from the .NET framework which have to be sent using the format <see cref="Formats.Binary"/>.
    /// </summary>
    public static class FormatBinaryExtensions
    {
        /// <summary>
        /// This takes the <paramref name="span"/>s SpanContext and injects it for propagation within the given <paramref name="data"/>
        /// using the format <see cref="Formats.Binary"/>. 
        /// </summary>
        /// <param name="tracer">A <see cref="ITracer"/> instance.</param>
        /// <param name="span">The Span instance's SpanContext will be injected into the carrier.</param>
        /// <param name="data">The carrier object which should be used for the propagation.</param>
        public static void InjectBinary(this ITracer tracer, ISpan span, byte[] data)
        {
            InjectBinary(tracer, span?.Context, data);
        }

        /// <summary>
        /// This takes the <paramref name="spanContext"/> and injects it for propagation within the given <paramref name="data"/>
        /// using the format <see cref="Formats.Binary"/>. 
        /// </summary>
        /// <param name="tracer">A <see cref="ITracer"/> instance.</param>
        /// <param name="spanContext">The SpanContext to inject into the carrier.</param>
        /// <param name="data">The carrier object which should be used for the propagation.</param>
        public static void InjectBinary(this ITracer tracer, ISpanContext spanContext, byte[] data)
        {
            if (tracer == null)
            {
                throw new ArgumentNullException(nameof(tracer));
            }

            tracer.Inject(spanContext, Formats.Binary, data);
        }

        /// <summary>
        /// Returns a new <see cref="ISpanContext"/> containing the baggage of the given <paramref name="data"/>
        /// (using the format <see cref="Formats.HttpHeaders"/>), or null if there was no baggage found.
        /// </summary>
        /// <param name="tracer">A <see cref="ITracer"/> instance.</param>
        /// <param name="data">The data will be used as a carrier for the propagation.</param>
        public static ISpanContext ExtractBinary(this ITracer tracer, byte[] data)
        {
            if (tracer == null)
            {
                throw new ArgumentNullException(nameof(tracer));
            }

            return tracer.Extract(Formats.Binary, data);
        }
    }
}