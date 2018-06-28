using System;
using System.Collections.Generic;
using System.Text;
using OpenTracing.Tag;

namespace OpenTracing.Decorators
{
    sealed class SpanFactoryDecorator : ISpan
    {
        private readonly ISpan _span;
        private readonly SpanContextDecoratorFactory _spanContextDecoratorFactory;

        public SpanFactoryDecorator(ISpan span, SpanContextDecoratorFactory spanContextDecoratorFactory)
        {
            _span = span;
            _spanContextDecoratorFactory = spanContextDecoratorFactory ?? throw new ArgumentNullException(nameof(spanContextDecoratorFactory));
        }

        public ISpanContext Context => _spanContextDecoratorFactory(_span.Context);

        public void Finish() => _span.Finish();

        public void Finish(DateTimeOffset finishTimestamp) => _span.Finish(finishTimestamp);

        public string GetBaggageItem(string key) => _span.GetBaggageItem(key);

        public ISpan Log(IEnumerable<KeyValuePair<string, object>> fields) { _span.Log(fields); return this; }

        public ISpan Log(DateTimeOffset timestamp, IEnumerable<KeyValuePair<string, object>> fields) { _span.Log(timestamp, fields); return this; }

        public ISpan Log(string @event) { _span.Log(@event); return this; }

        public ISpan Log(DateTimeOffset timestamp, string @event) { _span.Log(timestamp, @event); return this; }

        public ISpan SetBaggageItem(string key, string value) { _span.SetBaggageItem(key, value); return this; }

        public ISpan SetOperationName(string operationName) { _span.SetOperationName(operationName); return this; }

        public ISpan SetTag(string key, string value) { _span.SetTag(key, value); return this; }

        public ISpan SetTag(string key, bool value) { _span.SetTag(key, value); return this; }

        public ISpan SetTag(string key, int value) { _span.SetTag(key, value); return this; }

        public ISpan SetTag(string key, double value) { _span.SetTag(key, value); return this; }

        public ISpan SetTag(BooleanTag tag, bool value) { _span.SetTag(tag, value); return this; }

        public ISpan SetTag(IntOrStringTag tag, string value) { _span.SetTag(tag, value); return this; }

        public ISpan SetTag(IntTag tag, int value) { _span.SetTag(tag, value); return this; }

        public ISpan SetTag(StringTag tag, string value) { _span.SetTag(tag, value); return this; }
    }
}
