using System;
using System.Collections.Generic;

namespace OpenTracing.Noop
{
    internal sealed class NoopSpan : ISpan
    {
        internal static readonly NoopSpan Instance = new NoopSpan();

        public ISpanContext Context => NoopSpanContext.Instance;

        private NoopSpan()
        {
        }

        public void Finish()
        {
        }

        public void Finish(DateTimeOffset finishTimestamp)
        {
        }

        public ISpan SetTag(string key, string value)
        {
            return this;
        }

        public ISpan SetTag(string key, bool value)
        {
            return this;
        }

        public ISpan SetTag(string key, int value)
        {
            return this;
        }

        public ISpan SetTag(string key, double value)
        {
            return this;
        }

        public ISpan Log(IDictionary<string, object> fields)
        {
            return this;
        }

        public ISpan Log(DateTimeOffset timestamp, IDictionary<string, object> fields)
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

        public ISpan SetOperationName(string operationName)
        {
            return this;
        }

        public override string ToString()
        {
            return nameof(NoopSpan);
        }
    }
}
