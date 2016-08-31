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

        public void Inject<TCarrier>(ISpanContext spanContext, Format<TCarrier> format, TCarrier carrier)
        {
            // TODO add other formats (and maybe don't use if/else :D )

            var typedContext = (SpanContext)spanContext;

            if (format.Equals(Formats.TextMap))
            {
                _textMapCarrierHandler.MapContextToCarrier(typedContext, (ITextMap) carrier);
            }
            else
            {
                throw new UnsupportedFormatException($"The format '{format}' is not supported.");
            }
        }

        public ISpanContext Extract<TCarrier>(Format<TCarrier> format, TCarrier carrier)
        {
            // TODO add other formats (and maybe don't use if/else :D )

            if (format.Equals(Formats.TextMap))
            {
                return _textMapCarrierHandler.MapCarrierToContext((ITextMap) carrier);
            }
            
            throw new UnsupportedFormatException($"The format '{format}' is not supported.");
        }
    }
}
