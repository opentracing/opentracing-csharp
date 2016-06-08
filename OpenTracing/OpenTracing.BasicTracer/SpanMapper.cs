using OpenTracing.BasicTracer.Context;
using OpenTracing.Propagation;
using System;

namespace OpenTracing.BasicTracer.OpenTracingContext
{
    public class SpanMapper<T, TFormat> : ISpanMapper<TFormat> where T : ISpanContext
    {
        private const string prefixTracerState = "ot-tracer-";
        private const string prefixBaggage = "ot-baggage-";

        private const string fieldNameTraceID = prefixTracerState + "traceid";
        private const string fieldNameSpanID = prefixTracerState + "spanid";
        private const string fieldNameSampled = prefixTracerState + "sampled";

        private readonly Tracer<T> _tracer;
        private readonly IContextMapper<T, TFormat> _contextMapper;

        public SpanMapper(Tracer<T> tracer, IContextMapper<T, TFormat> contextMapper)
        {
            _contextMapper = contextMapper;
            _tracer = tracer;
        }

        public Span<T> ConvertToBasicTracerSpan(ISpan span)
        {
            if (!(span is Span<T>))
            {
                throw new System.Exception("Invalid span type");
            }

            return span as Span<T>;
        }

        public TFormat MapFrom(ISpan span)
        {
            var spanContext = ConvertToBasicTracerSpan(span).GetSpanContext();

            return _contextMapper.MapFrom(spanContext);
        }

        public bool TryMapTo(string operationName, TFormat data, out ISpan span)
        {          
            span = null;
            T spanContext;
            bool success = _contextMapper.TryMapTo(data, out spanContext);

            if (!success)
            {
                return false;
            }
            

            span = _tracer.NewSpan(spanContext, operationName, DateTime.Now);

            return true;
        }
    }
}
