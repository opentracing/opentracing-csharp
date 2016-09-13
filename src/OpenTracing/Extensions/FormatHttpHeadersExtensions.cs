using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using OpenTracing.Propagation;

namespace OpenTracing
{
    /// <summary>
    /// Contains <see cref="ITracer.Inject"/> and <see cref="ITracer.Extract"/> extension methods for types 
    /// from the .NET framework which have to be sent using the format <see cref="Formats.HttpHeaders"/>.
    /// </summary>
    public static class FormatHttpHeadersExtensions
    {
        /// <summary>
        /// This takes the <paramref name="span"/>s SpanContext and injects it for propagation within the given <paramref name="request"/>
        /// using the format <see cref="Formats.HttpHeaders"/>. 
        /// </summary>
        /// <param name="tracer">A <see cref="ITracer"/> instance.</param>
        /// <param name="span">The Span instance's SpanContext will be injected into the carrier.</param>
        /// <param name="request">The headers of the given http request will be used as a carrier for the propagation.</param>
        public static void InjectHttpHeaders(this ITracer tracer, ISpan span, HttpRequestMessage request)
        {
            InjectHttpHeaders(tracer, span?.Context, request?.Headers);
        }

        /// <summary>
        /// This takes the <paramref name="spanContext"/> and injects it for propagation within the given <paramref name="request"/>
        /// using the format <see cref="Formats.HttpHeaders"/>. 
        /// </summary>
        /// <param name="tracer">A <see cref="ITracer"/> instance.</param>
        /// <param name="spanContext">The SpanContext to inject into the carrier.</param>
        /// <param name="request">The headers of the given http request will be used as a carrier for the propagation.</param>
        public static void InjectHttpHeaders(this ITracer tracer, ISpanContext spanContext, HttpRequestMessage request)
        {
            InjectHttpHeaders(tracer, spanContext, request?.Headers);
        }

        /// <summary>
        /// This takes the <paramref name="span"/>s SpanContext and injects it for propagation within the given <paramref name="headers"/>
        /// using the format <see cref="Formats.HttpHeaders"/>. 
        /// </summary>
        /// <param name="tracer">A <see cref="ITracer"/> instance.</param>
        /// <param name="span">The Span instance's SpanContext will be injected into the carrier.</param>
        /// <param name="headers">The headers will be used as a carrier for the propagation.</param>
        public static void InjectHttpHeaders(this ITracer tracer, ISpan span, HttpHeaders headers)
        {
            InjectHttpHeaders(tracer, span?.Context, headers);
        }

        /// <summary>
        /// This takes the <paramref name="spanContext"/> and injects it for propagation within the given <paramref name="headers"/>
        /// using the format <see cref="Formats.HttpHeaders"/>. 
        /// </summary>
        /// <param name="tracer">A <see cref="ITracer"/> instance.</param>
        /// <param name="spanContext">The SpanContext to inject into the carrier.</param>
        /// <param name="headers">The headers will be used as a carrier for the propagation.</param>
        public static void InjectHttpHeaders(this ITracer tracer, ISpanContext spanContext, HttpHeaders headers)
        {
            if (tracer == null)
            {
                throw new ArgumentNullException(nameof(tracer));
            }

            tracer.Inject(spanContext, Formats.HttpHeaders, new SystemNetHttpHeadersCarrier(headers));
        }

        /// <summary>
        /// This takes the <paramref name="spanContext"/> and injects it for propagation within the given <paramref name="headers"/>
        /// using the format <see cref="Formats.HttpHeaders"/>. 
        /// </summary>
        /// <param name="tracer">A <see cref="ITracer"/> instance.</param>
        /// <param name="spanContext">The SpanContext to inject into the carrier.</param>
        /// <param name="headers">The carrier object which should be used for the propagation.</param>
        public static void InjectHttpHeaders(this ITracer tracer, ISpanContext spanContext, IDictionary<string, string> headers)
        {
            if (tracer == null)
            {
                throw new ArgumentNullException(nameof(tracer));
            }

            tracer.Inject(spanContext, Formats.HttpHeaders, new DictionaryCarrier(headers));
        }

        /// <summary>
        /// Returns a new <see cref="ISpanContext"/> containing the baggage of the given <paramref name="response"/>'s headers
        /// (using the format <see cref="Formats.HttpHeaders"/>), or null if there was no baggage found.
        /// </summary>
        /// <param name="tracer">A <see cref="ITracer"/> instance.</param>
        /// <param name="response">The headers of the response will be used as a carrier for the propagation.</param>
        public static ISpanContext ExtractHttpHeaders(this ITracer tracer, HttpResponseMessage response)
        {
            return ExtractHttpHeaders(tracer, response?.Headers);
        }

        /// <summary>
        /// Returns a new <see cref="ISpanContext"/> containing the baggage of the given <paramref name="headers"/>
        /// (using the format <see cref="Formats.HttpHeaders"/>), or null if there was no baggage found.
        /// </summary>
        /// <param name="tracer">A <see cref="ITracer"/> instance.</param>
        /// <param name="headers">The headers will be used as a carrier for the propagation.</param>
        public static ISpanContext ExtractHttpHeaders(this ITracer tracer, HttpHeaders headers)
        {
            if (tracer == null)
            {
                throw new ArgumentNullException(nameof(tracer));
            }

            return tracer.Extract(Formats.HttpHeaders, new SystemNetHttpHeadersCarrier(headers));
        }

        /// <summary>
        /// Returns a new <see cref="ISpanContext"/> containing the baggage of the given <paramref name="headers"/>
        /// (using the format <see cref="Formats.HttpHeaders"/>), or null if there was no baggage found.
        /// </summary>
        /// <param name="tracer">A <see cref="ITracer"/> instance.</param>
        /// <param name="headers">The headers will be used as a carrier for the propagation.</param>
        public static ISpanContext ExtractHttpHeaders(this ITracer tracer, IDictionary<string, string> headers)
        {
            if (tracer == null)
            {
                throw new ArgumentNullException(nameof(tracer));
            }

            return tracer.Extract(Formats.HttpHeaders, new DictionaryCarrier(headers));
        }
    }
}