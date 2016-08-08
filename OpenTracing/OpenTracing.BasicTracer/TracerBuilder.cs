using OpenTracing.BasicTracer.Context;
using OpenTracing.BasicTracer.OpenTracingContext;
using System;
using System.Collections.Generic;

namespace OpenTracing.BasicTracer
{
    public class TracerBuilder<T> where T : Context.ISpanContext
    {
        private ISpanContextFactory<T> _spanContextFactory = null;
        private ISpanRecorder<T> _spanRecorder = null;

        public TracerBuilder()
        {       
        }

        public TracerBuilder<T> SetSpanContextFactory(ISpanContextFactory<T> spanContextFactory)
        {
            _spanContextFactory = spanContextFactory;
            return this;
        }
        public TracerBuilder<T> SetSpanRecorder(ISpanRecorder<T> spanRecorder)
        {
            _spanRecorder = spanRecorder;
            return this;
        }

        public Tracer<T> BuildTracer()
        {
            if (_spanContextFactory == null)
            {
                throw new ArgumentNullException("No span context factory set.");
            }

            var mappers = new List<object>
            {
                { new OpenTracingSpanContextToTextMapper() }
            };

            return new Tracer<T>(_spanContextFactory, _spanRecorder, mappers);
        }
    }
}