using OpenTracing.BasicTracer.Context;
using OpenTracing.BasicTracer.OpenTracingContext;
using System;
using System.Collections.Generic;

namespace OpenTracing.BasicTracer
{
    public class TracerBuilder<TContext> where TContext : Context.ISpanContext
    {
        private ISpanContextFactory<TContext> _spanContextFactory = null;
        private ISpanRecorder<TContext> _spanRecorder = null;

        public TracerBuilder()
        {       
        }

        public TracerBuilder<TContext> SetSpanContextFactory(ISpanContextFactory<TContext> spanContextFactory)
        {
            _spanContextFactory = spanContextFactory;
            return this;
        }
        public TracerBuilder<TContext> SetSpanRecorder(ISpanRecorder<TContext> spanRecorder)
        {
            _spanRecorder = spanRecorder;
            return this;
        }

        public Tracer<TContext> BuildTracer()
        {
            if (_spanContextFactory == null)
            {
                throw new ArgumentNullException("No span context factory set.");
            }

            var mappers = new List<object>
            {
                { new OpenTracingSpanContextToTextMapper() }
            };

            return new Tracer<TContext>(_spanContextFactory, _spanRecorder, mappers);
        }
    }
}