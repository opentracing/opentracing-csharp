using System;
using System.Collections.Generic;
using System.Text;
using OpenTracing.Tag;

namespace OpenTracing.Decorators
{
    public class SpanBuilderDecorator : ISpanBuilder
    {
        private readonly ISpanBuilder _spanBuilder;

        public SpanBuilderDecorator(ISpanBuilder spanBuilder)
        {
            _spanBuilder = spanBuilder;
        }

        public virtual ISpanBuilder AddReference(string referenceType, ISpanContext referencedContext) { _spanBuilder.AddReference(referenceType, referencedContext); return this; }

        public virtual ISpanBuilder AsChildOf(ISpanContext parent) { _spanBuilder.AsChildOf(parent); return this; }

        public virtual ISpanBuilder AsChildOf(ISpan parent) { _spanBuilder.AsChildOf(parent); return this; }

        public virtual ISpanBuilder IgnoreActiveSpan() { _spanBuilder.IgnoreActiveSpan(); return this; }

        public virtual ISpan Start() => _spanBuilder.Start();

        public virtual IScope StartActive() => _spanBuilder.StartActive();

        public virtual IScope StartActive(bool finishSpanOnDispose) => _spanBuilder.StartActive(finishSpanOnDispose);

        public virtual ISpanBuilder WithStartTimestamp(DateTimeOffset timestamp) { _spanBuilder.WithStartTimestamp(timestamp); return this; }

        public virtual ISpanBuilder WithTag(string key, string value) { _spanBuilder.WithTag(key, value); return this; }

        public virtual ISpanBuilder WithTag(string key, bool value) { _spanBuilder.WithTag(key, value); return this; }

        public virtual ISpanBuilder WithTag(string key, int value) { _spanBuilder.WithTag(key, value); return this; }

        public virtual ISpanBuilder WithTag(string key, double value) { _spanBuilder.WithTag(key, value); return this; }

        public virtual ISpanBuilder WithTag(BooleanTag tag, bool value) { _spanBuilder.WithTag(tag, value); return this; }

        public virtual ISpanBuilder WithTag(IntOrStringTag tag, string value) { _spanBuilder.WithTag(tag, value); return this; }

        public virtual ISpanBuilder WithTag(IntTag tag, int value) { _spanBuilder.WithTag(tag, value); return this; }

        public virtual ISpanBuilder WithTag(StringTag tag, string value) { _spanBuilder.WithTag(tag, value); return this; }
    }
}
