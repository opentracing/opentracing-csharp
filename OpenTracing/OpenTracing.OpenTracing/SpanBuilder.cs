using System;
using System.Collections.Generic;

namespace OpenTracing
{
    public class SpanBuilder
    {
        private ISpanFactory _spanFactory;

        private string _operationName;
        private DateTime? _startTime;
        private Dictionary<string, string> _tags { get; set; } = new Dictionary<string, string>() { };

        private List<SpanReference> _references = new List<SpanReference> { };

        public SpanBuilder(ISpanFactory spanFactory, string operationName)
        {
            _spanFactory = spanFactory;
            _operationName = operationName;
        }

        public SpanBuilder AsChildOf(ISpanContext spanContext)
        {
            _references.Add(SpanReference.ChildOf(spanContext));
            return this;
        }

        public SpanBuilder AsChildOf(ISpan span)
        {
            return AsChildOf(span.GetSpanContext());
        }

        public SpanBuilder FollowsFrom(ISpanContext spanContext)
        {
            _references.Add(SpanReference.FollowsFrom(spanContext));
            return this;
        }

        public SpanBuilder FollowsFrom(ISpan span)
        {
            return AsChildOf(span.GetSpanContext());
        }

        public SpanBuilder WithTag(string key, string value)
        {
            _tags[key] = value; 
            return this;
        }

        public SpanBuilder WithTag(string key, int value)
        {
            _tags[key] = value.ToString();
            return this;
        }

        public SpanBuilder WithTag(string key, bool value)
        {
            _tags[key] = value.ToString();
            return this;
        }

        public SpanBuilder WithStartTime(DateTime startTime)
        {
            _startTime = startTime;
            return this;
        }

        public ISpan Start()
        {
            return _spanFactory.StartSpan(
                new StartSpanOptions()
                {
                    OperationName = _operationName,
                    StartTime = _startTime ?? DateTime.Now,
                    Tag = _tags,
                    References = _references,
                });
        }
    }
}
