using System;
using System.Collections.Generic;
using System.Text;
using OpenTracing.Tag;

namespace OpenTracing.Decorators
{
    public class SpanDecorator : ISpan
    {
        private readonly ISpan _span;
        private readonly SpanContextDecoratorFactory _spanContextDecoratorFactory;

        public SpanDecorator(ISpan span, SpanContextDecoratorFactory spanContextDecoratorFactory)
        {
            _span = span;
            _spanContextDecoratorFactory = spanContextDecoratorFactory;
        }

        public virtual ISpanContext Context => _spanContextDecoratorFactory(_span.Context);

        public virtual void Finish() => _span.Finish();

        public virtual void Finish(DateTimeOffset finishTimestamp) => _span.Finish(finishTimestamp);

        public virtual string GetBaggageItem(string key) => _span.GetBaggageItem(key);

        public virtual ISpan Log(IEnumerable<KeyValuePair<string, object>> fields) { _span.Log(fields); return this; }

        public virtual ISpan Log(DateTimeOffset timestamp, IEnumerable<KeyValuePair<string, object>> fields) { _span.Log(timestamp, fields); return this; }

        public virtual ISpan Log(string @event) { _span.Log(@event); return this; }

        public virtual ISpan Log(DateTimeOffset timestamp, string @event) { _span.Log(timestamp, @event); return this; }

        public virtual ISpan SetBaggageItem(string key, string value) { _span.SetBaggageItem(key, value); return this; }

        public virtual ISpan SetOperationName(string operationName) { _span.SetOperationName(operationName); return this; }

        public virtual ISpan SetTag(string key, string value) { _span.SetTag(key, value); return this; }

        public virtual ISpan SetTag(string key, bool value) { _span.SetTag(key, value); return this; }

        public virtual ISpan SetTag(string key, int value) { _span.SetTag(key, value); return this; }

        public virtual ISpan SetTag(string key, double value) { _span.SetTag(key, value); return this; }

        public virtual ISpan SetTag(BooleanTag tag, bool value) { _span.SetTag(tag, value); return this; }

        public virtual ISpan SetTag(IntOrStringTag tag, string value) { _span.SetTag(tag, value); return this; }

        public virtual ISpan SetTag(IntTag tag, int value) { _span.SetTag(tag, value); return this; }

        public virtual ISpan SetTag(StringTag tag, string value) { _span.SetTag(tag, value); return this; }
    }
}
