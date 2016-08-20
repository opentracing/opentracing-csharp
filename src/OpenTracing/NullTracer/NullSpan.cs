using System;

namespace OpenTracing.NullTracer
{
    public class NullSpan : ISpan
    {
        internal static readonly NullSpan Instance = new NullSpan(NullTracer.Instance, NullSpanContext.Instance);

        private readonly ITracer _tracer;
        private readonly ISpanContext _context;

        private NullSpan(ITracer tracer, ISpanContext context)
        {
            _tracer = tracer;
            _context = context;
        }

        public ISpanContext Context => _context;

        public ITracer Tracer => _tracer;

        public ISpan SetOperationName(string operationName)
        {
            return this;
        }

        public ISpan AddReference(string referenceType, ISpanContext spanContext)
        {
            return this;
        }

        public ISpan SetTag(string key, object value)
        {
            return this;
        }

        public ISpan LogEvent(string eventName, object payload = null)
        {
            return this;
        }

        public ISpan LogEvent(DateTimeOffset timestamp, string eventName, object payload = null)
        {
            return this;
        }

        public string GetBaggageItem(string key)
        {
            return null;
        }

        public ISpan SetBaggageItem(string key, string value)
        {
            return this;
        }

        public void Finish()
        {
        }

        public void Finish(DateTimeOffset finishTimestamp)
        {
        }
    }
}