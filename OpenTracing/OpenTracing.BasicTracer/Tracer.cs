using OpenTracing.BasicTracer.Context;
using OpenTracing.Propagation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenTracing.BasicTracer
{
    public class Tracer<T> : ITracer where T : ISpanContext
    {
        private readonly ISpanContextFactory<T> _spanContextFactory;
        private ISpanRecorder<T> _spanRecorder;
        private IList<object> _mappers;

        internal Tracer(ISpanContextFactory<T> spanContextFactory, ISpanRecorder<T> spanRecorder, IList<object> mappers)
        {
            _spanContextFactory = spanContextFactory;
            _spanRecorder = spanRecorder;
            _mappers = mappers;
        }

        private Span<T> ConvertToBasicTracerSpan(ISpan span)
        {
            if (!(span is Span<T>))
            {
                throw new System.Exception("Invalid span type");
            }

            return span as Span<T>;
        }

        public void Inject<TFormat>(ISpan span, IInjectCarrier<TFormat> carrier)
        {
            var mapper = _mappers.OfType<IContextMapper<T, TFormat>>().FirstOrDefault();

            if (mapper == null)
            {
                throw new Exception("Could not find mapper");
            }

            var spanContext = ConvertToBasicTracerSpan(span).GetSpanContext();

            carrier.MapFrom(mapper.MapFrom(spanContext));
        }

        public bool TryJoin<TFormat>(string operationName, IExtractCarrier<TFormat> carrier, out ISpan span)
        {
            span = null;

            var mapper = _mappers.OfType<IContextMapper<T, TFormat>>().FirstOrDefault();

            if (mapper == null)
            {
                throw new Exception("Could not find mapper");
            }

            TFormat format;

            var success = carrier.TryMapTo(out format);

            if (!success)
            {
                return false;
            }

            T spanContext;

            success = mapper.TryMapTo(format, out spanContext);

            if (!success)
            {
                return false;
            }
                        
            span = NewSpan(spanContext, operationName, DateTime.Now);

            return true;
        }

        public ISpan StartSpan(StartSpanOptions startSpanOptions)
        {
            ISpan span;

            var rootSpanContext = _spanContextFactory.NewRootSpanContext();

            span = NewSpan(rootSpanContext, startSpanOptions.OperationName, startSpanOptions.StartTime);

            foreach (var tag in startSpanOptions.Tag)
            {
                span.SetTag(tag.Key, tag.Value);
            }

            return span;
        }

        public ISpan StartSpan(string operationName)
        {
            return StartSpan(new StartSpanOptions()
            {
                OperationName = operationName,
            });
        }

        internal ISpan NewSpan(T spanContext, string operationName, DateTime startTime)
        {
            return new Span<T>(this, _spanRecorder, spanContext, operationName, startTime);
        }
    }
}
