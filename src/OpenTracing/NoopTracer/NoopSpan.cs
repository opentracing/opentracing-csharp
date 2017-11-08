namespace OpenTracing.NoopTracer
{
    using System;
    using System.Collections.Generic;

    internal sealed class NoopSpan : ISpan
    {
        public static ISpan Instance = new NoopSpan();

        private NoopSpan()
        {
        }

        public void Dispose()
        {
        }

        public ISpanContext GetSpanContext()
        {
            return NoopSpanContext.Instance;
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

        public ISpan Log(string eventName)
        {
            return this;
        }

        public ISpan Log(DateTimeOffset timestamp, string eventName)
        {
            return this;
        }

        public ISpan SetBaggageItem(string key, string value)
        {
            return this;
        }

        public bool TryGetBaggageItem(string key, out string value)
        {
            value = default(string);
            return false;
        }

        public void Finish()
        {
        }

        public void Finish(DateTimeOffset finishTimestamp)
        {
        }
    }
}