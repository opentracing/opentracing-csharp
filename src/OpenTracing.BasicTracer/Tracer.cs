using OpenTracing.Propagation;
using System;
using OpenTracing.BasicTracer.Propagation;

namespace OpenTracing.BasicTracer
{
    public class Tracer : ITracer
    {
        private readonly ISpanContextFactory _spanContextFactory;
        private readonly ISpanRecorder _spanRecorder;

        private readonly TextMapCarrierHandler _textMapCarrierHandler = new TextMapCarrierHandler();

        public Tracer(
            ISpanContextFactory spanContextFactory,
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

        public ISpan StartSpan(string operationName)
        {
            return StartSpan(operationName, null);
        }

        public ISpan StartSpan(string operationName, StartSpanOptions options)
        {
            options = options ?? new StartSpanOptions();

            var spanContext = _spanContextFactory.CreateSpanContext(options.References);
            var startTimestamp = options.StartTimestamp ?? DateTimeOffset.UtcNow;

            var span = new Span(_spanRecorder, spanContext, operationName, startTimestamp);

            return span;
        }

        public void Inject(ISpanContext spanContext, string format, IInjectCarrier carrier)
        {
            switch (format)
            {
                case Formats.TextMap:
                    InjectTextMap(spanContext, carrier);
                    break;
                // TODO add other formats
                default:
                    throw new UnsupportedFormatException($"The format '{format}' is not supported.");
            }
        }

        private void InjectTextMap(ISpanContext spanContext, IInjectCarrier carrier)
        {
            var typedContext = (SpanContext)spanContext;

            var textMapCarrier = carrier as ITextMapCarrier;
            if (textMapCarrier != null)
            {
                _textMapCarrierHandler.MapContextToCarrier(typedContext, textMapCarrier);
                return;
            }

            throw new InvalidCarrierException($"The carrier '{carrier.GetType()}' is not supported for the format '{Formats.TextMap}'.");
        }


        public ISpanContext Extract(string format, IExtractCarrier carrier)
        {
            switch (format)
            {
                case Formats.TextMap:
                    return ExtractTextMap(carrier);
                // TODO add other formats
                default:
                    throw new UnsupportedFormatException($"The format '{format}' is not supported.");
            }
        }

        private ISpanContext ExtractTextMap(IExtractCarrier carrier)
        {
            var textMapCarrier = carrier as ITextMapCarrier;
            if (textMapCarrier != null)
            {
                return _textMapCarrierHandler.MapCarrierToContext(textMapCarrier);
            }

            throw new InvalidCarrierException($"The carrier '{carrier.GetType()}' is not supported for the format '{Formats.TextMap}'.");
        }
    }
}
