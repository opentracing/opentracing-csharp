using System;
using System.Collections.Generic;
using OpenTracing.Propagation;

namespace OpenTracing
{
    /// <summary>
    /// Contains <see cref="ITracer.Inject"/> and <see cref="ITracer.Extract"/> extension methods for types 
    /// from the .NET framework which have to be sent using the format <see cref="Formats.TextMap"/>.
    /// </summary>
    public static class FormatTextMapExtensions
    {
        /// <summary>
        /// This takes the SpanContext and injects it for propagation within the given <paramref name="data"/>
        /// using the format <see cref="Formats.TextMap"/>. 
        /// </summary>
        /// <param name="tracer">A <see cref="ITracer"/> instance.</param>
        /// <param name="span">The Span's SpanContext will be injected into the carrier.</param>
        /// <param name="data">The carrier object which should be used for the propagation.</param>
        public static void InjectTextMap(this ITracer tracer, ISpan span, IDictionary<string, string> data)
        {
            InjectTextMap(tracer, span?.Context, data);
        }

        /// <summary>
        /// This takes the SpanContext and injects it for propagation within the given <paramref name="data"/>
        /// using the format <see cref="Formats.TextMap"/>. 
        /// </summary>
        /// <param name="tracer">A <see cref="ITracer"/> instance.</param>
        /// <param name="spanContext">The SpanContext to inject into the carrier.</param>
        /// <param name="data">The carrier object which should be used for the propagation.</param>
        public static void InjectTextMap(this ITracer tracer, ISpanContext spanContext, IDictionary<string, string> data)
        {
            if (tracer == null)
            {
                throw new ArgumentNullException(nameof(tracer));
            }

            tracer.Inject(spanContext, Formats.TextMap, new DictionaryCarrier(data));
        }

        /// <summary>
        /// Returns a new <see cref="ISpanContext"/> containing the baggage of the given <paramref name="data"/>
        /// (using the format <see cref="Formats.HttpHeaders"/>), or null if there was no baggage found.
        /// </summary>
        /// <param name="tracer">A <see cref="ITracer"/> instance.</param>
        /// <param name="data">The dictionary will be used as a carrier for the propagation.</param>
        public static ISpanContext ExtractTextMap(this ITracer tracer, IDictionary<string, string> data)
        {
            if (tracer == null)
            {
                throw new ArgumentNullException(nameof(tracer));
            }

            return tracer.Extract(Formats.TextMap, new DictionaryCarrier(data));
        }
    }
}