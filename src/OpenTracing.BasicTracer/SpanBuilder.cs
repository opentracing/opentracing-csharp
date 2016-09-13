using System;
using System.Collections.Generic;

namespace OpenTracing.BasicTracer
{
    /// <summary>
    /// Helper builder for convenience to construct startoptions and create a new span.
    /// </summary>
    public class SpanBuilder : ISpanBuilder
    {
        private readonly Tracer _tracer;
        private readonly string _operationName;

        // not initialized to save allocations in case there are no references.
        private List<Tuple<string, ISpanContext>> _references;

        // not initialized to save allocations in case there are no tags.
        private IDictionary<string, object> _tags;
        
        private DateTime? _startTimestamp;

        public SpanBuilder(Tracer tracer, string operationName)
        {
            if (tracer == null)
            {
                throw new ArgumentNullException(nameof(tracer));
            }
            if (string.IsNullOrWhiteSpace(operationName))
            {
                throw new ArgumentNullException(nameof(operationName));
            }

            _tracer = tracer;
            _operationName = operationName;
        }

        public ISpanBuilder AsChildOf(ISpanContext spanContext)
        {
            return AddReference(References.ChildOf, spanContext);
        }

        public ISpanBuilder AsChildOf(ISpan span)
        {
            return AsChildOf(span?.Context);
        }

        public ISpanBuilder FollowsFrom(ISpanContext spanContext)
        {
            return AddReference(References.FollowsFrom, spanContext);
        }

        public ISpanBuilder FollowsFrom(ISpan span)
        {
            return FollowsFrom(span?.Context);
        }

        public ISpanBuilder AddReference(string referenceType, ISpanContext referencedContext)
        {
            if (string.IsNullOrWhiteSpace(referenceType))
            {
                throw new ArgumentNullException(nameof(referenceType));
            }

            if (referencedContext != null)
            {
                if (_references == null)
                {
                    _references = new List<Tuple<string, ISpanContext>>();
                }

                _references.Add(Tuple.Create(referenceType, referencedContext));
            }

            return this;
        }

        public ISpanBuilder WithTag(string key, object value)
        {
            if (_tags == null)
            {
                _tags = new Dictionary<string, object>();
            }

            _tags[key] = value;
            return this;
        }

        public ISpanBuilder WithStartTimestamp(DateTime startTimestamp)
        {
            _startTimestamp = startTimestamp;
            return this;
        }

        public ISpan Start()
        {
            return _tracer.StartSpan(_operationName, _startTimestamp, _references, _tags);
        }
    }
}