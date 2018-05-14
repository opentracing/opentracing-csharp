using System;
using System.Collections.Generic;
using System.Linq;
using OpenTracing.Tag;

namespace OpenTracing.Mock
{
    public sealed class MockSpanBuilder : ISpanBuilder
    {
        private readonly MockTracer _tracer;
        private readonly string _operationName;
        private DateTimeOffset _startTimestamp = DateTimeOffset.MinValue;
        private readonly List<MockSpan.Reference> _references = new List<MockSpan.Reference>();
        private readonly Dictionary<string, object> _initialTags = new Dictionary<string, object>();
        private bool _ignoreActiveSpan;

        public MockSpanBuilder(MockTracer tracer, string operationName)
        {
            _tracer = tracer;
            _operationName = operationName;
        }

        public ISpanBuilder AsChildOf(ISpanContext parent)
        {
            if (parent == null)
                return this;

            return AddReference(References.ChildOf, parent);
        }

        public ISpanBuilder AsChildOf(ISpan parent)
        {
            if (parent == null)
                return this;

            return AddReference(References.ChildOf, parent.Context);
        }

        public ISpanBuilder AddReference(string referenceType, ISpanContext referencedContext)
        {
            if (referencedContext != null)
            {
                _references.Add(new MockSpan.Reference((MockSpanContext)referencedContext, referenceType));
            }

            return this;
        }

        public ISpanBuilder IgnoreActiveSpan()
        {
            _ignoreActiveSpan = true;
            return this;
        }

        public ISpanBuilder WithTag(string key, bool value)
        {
            _initialTags[key] = value;
            return this;
        }

        public ISpanBuilder WithTag(string key, double value)
        {
            _initialTags[key] = value;
            return this;
        }

        public ISpanBuilder WithTag(string key, int value)
        {
            _initialTags[key] = value;
            return this;
        }

        public ISpanBuilder WithTag(string key, string value)
        {
            _initialTags[key] = value;
            return this;
        }

        public ISpanBuilder WithTag(BooleanTag tag, bool value)
        {
            _initialTags[tag.Key] = value;
            return this;
        }

        public ISpanBuilder WithTag(IntOrStringTag tag, string value)
        {
            _initialTags[tag.Key] = value;
            return this;
        }

        public ISpanBuilder WithTag(IntTag tag, int value)
        {
            _initialTags[tag.Key] = value;
            return this;
        }

        public ISpanBuilder WithTag(StringTag tag, string value)
        {
            _initialTags[tag.Key] = value;
            return this;
        }

        public ISpanBuilder WithStartTimestamp(DateTimeOffset startTimestamp)
        {
            _startTimestamp = startTimestamp;
            return this;
        }

        public IScope StartActive()
        {
            return StartActive(finishSpanOnDispose: true);
        }

        public IScope StartActive(bool finishSpanOnDispose)
        {
            ISpan span = Start();
            return _tracer.ScopeManager.Activate(span, finishSpanOnDispose);
        }

        public ISpan Start()
        {
            if (_startTimestamp == DateTimeOffset.MinValue) // value was not set by builder
            {
                _startTimestamp = DateTimeOffset.UtcNow;
            }

            ISpanContext activeSpanContext = _tracer.ActiveSpan?.Context;

            if (!_references.Any() && !_ignoreActiveSpan && activeSpanContext != null)
            {
                _references.Add(new MockSpan.Reference((MockSpanContext)activeSpanContext, References.ChildOf));
            }

            return new MockSpan(_tracer, _operationName, _startTimestamp, _initialTags, _references);
        }
    }
}
