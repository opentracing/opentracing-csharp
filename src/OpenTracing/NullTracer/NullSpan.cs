using System;
using System.Collections.Generic;

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

        public ISpan SetTag(string key, int value)
        {
            return this;
        }

        public ISpan SetTag(string key, string value)
        {
            return this;
        }

        public ISpan Log(IEnumerable<KeyValuePair<string, object>> fields)
        {
            return this;
        }

        public ISpan Log(DateTimeOffset timestamp, IEnumerable<KeyValuePair<string, object>> fields)
        {
            return this;
        }

        public ISpan Log(string @event)
        {
            return this;
        }

        public ISpan Log(DateTimeOffset timestamp, string @event)
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

        public void Finish()
        {
        }

        public void Finish(DateTimeOffset finishTimestamp)
        {
        }

        public void Dispose()
        {
        }
    }
}