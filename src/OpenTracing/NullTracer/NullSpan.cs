using System;

namespace OpenTracing.NullTracer
{
    public class NullSpan : ISpan
    {
        private readonly ITracer _tracer;
        private readonly ISpanContext _context;

        public NullSpan(ITracer tracer)
        {
            _tracer = tracer;
            _context = new NullSpanContext();
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

        public ISpan SetTag(string key, string value)
        {
            return this;
        }

        public ISpan SetTag<T>(string key, T value) where T : struct
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

        public void Finish(DateTimeOffset finishTime)
        {
        }
    }
}