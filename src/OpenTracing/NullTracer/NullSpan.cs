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

        public ISpan SetTag(string key, bool value)
        {
            return this;
        }

        public ISpan SetTag(string key, double value)
        {
            return this;
        }

        public ISpan SetTag(string key, string value)
        {
            return this;
        }

        public ISpan Log(params LogField[] fields)
        {
            return this;
        }

        public ISpan Log(DateTime timestamp, params LogField[] fields)
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