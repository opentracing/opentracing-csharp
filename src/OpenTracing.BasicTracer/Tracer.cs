using OpenTracing.BasicTracer.Context;
using OpenTracing.Propagation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenTracing.BasicTracer
{
    public class Tracer : ITracer
    {
        private readonly ISpanContextFactory<SpanContext> _spanContextFactory;
        private readonly ISpanRecorder _spanRecorder;

        private readonly IList<object> _mappers = new List<object>
            {
                { new SpanContextToTextMapMapper() }
            };

        public Tracer(
            ISpanContextFactory<SpanContext> spanContextFactory,
            ISpanRecorder spanRecorder)
        {
            if (spanContextFactory == null)
            {
                throw new ArgumentNullException(nameof(spanContextFactory));
            }

            if (spanRecorder == null)
            {
                throw new ArgumentNullException(nameof(spanRecorder));
            }

            _spanContextFactory = spanContextFactory;
            _spanRecorder = spanRecorder;
        }

        public ISpanBuilder BuildSpan(string operationName)
        {
            return new SpanBuilder(this, operationName);
        }

        public ISpan StartSpan(
            string operationName,
            DateTimeOffset? startTimestamp,
            IList<SpanReference> references,
            IDictionary<string, object> tags)
        {
            SpanContext spanContext;

            if (!references?.Any() ?? true)
            {
                spanContext = _spanContextFactory.NewRootSpanContext();
            }
            else
            {
                spanContext = _spanContextFactory.NewChildSpanContext(references);
            }

            ISpan span = new Span(_spanRecorder, spanContext, operationName, startTimestamp ?? DateTimeOffset.UtcNow, tags);

            return span;
        }

        public void Inject<TFormat>(ISpanContext spanContext, IInjectCarrier<TFormat> carrier)
        {
            var typedContext = (SpanContext)spanContext;

            var mapper = _mappers.OfType<IContextMapper<TFormat>>().FirstOrDefault();

            if (mapper == null)
            {
                throw new UnsupportedFormatException($"The format '{typeof(TFormat)}' is not supported.");
            }

            carrier.MapFrom(mapper.MapFrom(typedContext));
        }

        public ISpanContext Extract<TFormat>(IExtractCarrier<TFormat> carrier)
        {
            var mapper = _mappers.OfType<IContextMapper<TFormat>>().FirstOrDefault();

            if (mapper == null)
            {
                throw new UnsupportedFormatException($"The format '{typeof(TFormat)}' is not supported.");
            }

            return mapper.MapTo(carrier.Extract());
        }
    }
}
