using OpenTracing.BasicTracer.Context;
using OpenTracing.Propagation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenTracing.BasicTracer
{
    public class Tracer<TContext> : ITracer where TContext : Context.ISpanContext
    {
        private readonly ISpanFactory _spanFactory;
        private IList<object> _mappers;

        internal Tracer(ISpanFactory spanFactory, IList<object> mappers)
        {
            _spanFactory = spanFactory;
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

        public void Inject<TFormat>(ISpanContext spanContext, IInjectCarrier<TFormat> carrier)
        {
            var mapper = _mappers.OfType<IContextMapper<TContext, TFormat>>().FirstOrDefault();

            if (mapper == null)
            {
                throw new Exception("Could not find mapper");
            }

            var basicTracerSpanContext = ConvertToBasicTracerSpan(spanContext);

            carrier.MapFrom(mapper.MapFrom(basicTracerSpanContext));
        }

        public ExtractResult Extract<TFormat>(string operationName, IExtractCarrier<TFormat> carrier)
        {
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

            var contextMapToResult = mapper.MapTo(extractCarrierResult.Context);

            if (!contextMapToResult.Success)
            {
                return new ExtractResult(contextMapToResult.MapException);
            }

            return new ExtractResult(contextMapToResult.SpanContext);
        }

        public ISpan StartSpan(string operationName, StartSpanOptions startSpanOptions)
        {
            return _spanFactory.StartSpan(operationName, startSpanOptions);
        }
    }
}
