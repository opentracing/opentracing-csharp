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

        public void Inject(ISpanContext spanContext, IInjectCarrier carrier)
        {
            var typedContext = (SpanContext)spanContext;

            var textMapCarrier = carrier as TextMapCarrier;
            if (textMapCarrier != null)
            {
                _textMapCarrierHandler.MapContextToCarrier(typedContext, textMapCarrier);
                return;
            }

            throw new NotSupportedException($"InjectCarrier type '{carrier.GetType()}' is not supported");
        }

        public ISpanContext Extract(IExtractCarrier carrier)
        {
            var textMapCarrier = carrier as TextMapCarrier;
            if (textMapCarrier != null)
            {
                return _textMapCarrierHandler.MapCarrierToContext(textMapCarrier);
            }

            throw new NotSupportedException($"ExtractCarrier type '{carrier.GetType()}' is not supported");
        }
    }
}
