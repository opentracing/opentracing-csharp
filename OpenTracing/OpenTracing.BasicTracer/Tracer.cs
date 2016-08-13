using OpenTracing.BasicTracer.Context;
using OpenTracing.Propagation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenTracing.BasicTracer
{
    public class Tracer<T> : ITracer where T : Context.ISpanContext
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

        private T ConvertToBasicTracerSpan(ISpanContext spanContext)
        {
            if (!(spanContext is T))
            {
                throw new System.Exception("Invalid span context type");
            }

            return (T)spanContext;
        }

        public void Inject<TFormat>(ISpan span, IInjectCarrier<TFormat> carrier)
        {
            var mapper = _mappers.OfType<IContextMapper<T, TFormat>>().FirstOrDefault();

            if (mapper == null)
            {
                throw new Exception("Could not find mapper");
            }

            var spanContext = ConvertToBasicTracerSpan(span.GetSpanContext());

            carrier.MapFrom(mapper.MapFrom(spanContext));
        }

        public ExtractResult Extract<TFormat>(string operationName, IExtractCarrier<TFormat> carrier)
        {
            ISpan span = null;

            var mapper = _mappers.OfType<IContextMapper<T, TFormat>>().FirstOrDefault();

            if (mapper == null)
            {
                throw new Exception("Could not find mapper");
            }

            var extractCarrierResult = carrier.Extract();

            if (!extractCarrierResult.Success)
            {
                return new ExtractResult(extractCarrierResult.ExtractException);
            }

            T spanContext;

            var success = mapper.TryMapTo(extractCarrierResult.FormatData, out spanContext);

            if (!success)
            {
                return new ExtractResult(new Exception("Error"));
            }
                        
            span = NewSpan(spanContext, operationName, DateTime.Now);

            return new ExtractResult(span);
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
