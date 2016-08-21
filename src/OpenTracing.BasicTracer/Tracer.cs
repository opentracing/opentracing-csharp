using OpenTracing.Propagation;
using System;

namespace OpenTracing.BasicTracer
{
    public class Tracer : ITracer
    {
        private readonly ISpanContextFactory _spanContextFactory;
        private readonly ISpanRecorder _spanRecorder;

        public Tracer(ISpanContextFactory spanContextFactory, ISpanRecorder spanRecorder)
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
            return StartSpan(operationName, DateTimeOffset.UtcNow);
        }

        public ISpan StartSpan(string operationName, DateTimeOffset startTimestamp)
        {
            var rootSpanContext = _spanContextFactory.NewRootSpanContext();
            var span = new Span(this, _spanRecorder, rootSpanContext, operationName, startTimestamp);
            return span;
        }

        public void Inject(ISpanContext spanContext, IInjectCarrier carrier)
        {
            throw new NotImplementedException();
            //carrier.MapFrom(span.GetSpanContext());
        }

        public ISpanContext Extract(IExtractCarrier carrier)
        {
            throw new NotImplementedException();

            // Span span = null;
            // SpanContext spanContext;

            // var couldExtractTraceInfo = carrier.TryMapTo(out spanContext);

            // if (!couldExtractTraceInfo)
            // {
            //     return false;
            // }

            // span = NewSpan(spanContext, operationName, DateTime.Now);

            // return true;
        }
    }
}
