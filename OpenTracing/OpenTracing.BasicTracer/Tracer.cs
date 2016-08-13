using OpenTracing.BasicTracer.Context;
using OpenTracing.Propagation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenTracing.BasicTracer
{
    public class Tracer<TContext> : ITracer where TContext : Context.ISpanContext
    {
        private readonly ISpanContextFactory<TContext> _spanContextFactory;
        private ISpanRecorder<TContext> _spanRecorder;
        private IList<object> _mappers;

        internal Tracer(ISpanContextFactory<TContext> spanContextFactory, ISpanRecorder<TContext> spanRecorder, IList<object> mappers)
        {
            _spanContextFactory = spanContextFactory;
            _spanRecorder = spanRecorder;
            _mappers = mappers;
        }

        private TContext ConvertToBasicTracerSpan(ISpanContext spanContext)
        {
            if (!(spanContext is TContext))
            {
                throw new System.Exception("Invalid span context type");
            }

            return (TContext)spanContext;
        }

        public void Inject<TFormat>(ISpan span, IInjectCarrier<TFormat> carrier)
        {
            var mapper = _mappers.OfType<IContextMapper<TContext, TFormat>>().FirstOrDefault();

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

            var mapper = _mappers.OfType<IContextMapper<TContext, TFormat>>().FirstOrDefault();

            if (mapper == null)
            {
                throw new Exception("Could not find mapper");
            }

            var extractCarrierResult = carrier.Extract();

            if (!extractCarrierResult.Success)
            {
                return new ExtractResult(extractCarrierResult.ExtractException);
            }

            var contextMapToResult = mapper.MapTo(extractCarrierResult.FormatData);

            if (!contextMapToResult.Success)
            {
                return new ExtractResult(contextMapToResult.MapException);
            }
                        
            span = NewSpan(contextMapToResult.SpanContext, operationName, DateTime.Now);

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

        internal ISpan NewSpan(TContext spanContext, string operationName, DateTime startTime)
        {
            return new Span<TContext>(this, _spanRecorder, spanContext, operationName, startTime);
        }
    }
}
