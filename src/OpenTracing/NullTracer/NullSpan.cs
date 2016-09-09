using System;

namespace OpenTracing.NullTracer
{
    public class NullSpan : ISpan
    {
        internal static readonly NullSpan Instance = new NullSpan();

        public ISpanContext Context => NullSpanContext.Instance;

        private NullSpan()
        {
        }

        public ISpan SetOperationName(string operationName)
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

        public ISpan LogEvent(DateTime timestamp, string eventName, object payload = null)
        {
            return this;
        }

        public ISpan SetBaggageItem(string key, string value)
        {
            return this;
        }

        public string GetBaggageItem(string key)
        {
            return null;
        }

        public void Finish(DateTime? finishTimestamp = null)
        {
        }

        public void Dispose()
        {
        }
    }
}