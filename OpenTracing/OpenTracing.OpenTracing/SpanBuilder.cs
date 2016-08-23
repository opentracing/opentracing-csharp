using System;
using System.Collections.Generic;

namespace OpenTracing
{
    /// <summary>
    /// Helper builder for convenience to construct startoptions and create a new span.
    /// </summary>
    public class SpanBuilder
    {
        private ITracer _tracer;

        private string _operationName;
        private DateTime? _startTime;
        private Dictionary<string, string> _tags { get; set; } = new Dictionary<string, string>() { };

        private List<SpanReference> _references = new List<SpanReference> { };

        public SpanBuilder(ITracer tracer, string operationName)
        {
            _tracer = tracer;
            _operationName = operationName;
        }

        /// <summary>
        /// Adds a Span Reference with a ChildOfRef to parent span.
        /// If spanContext == null, the option has no effect.
        /// </summary>
        /// <param name="spanContext">Parent span context</param>
        public SpanBuilder AsChildOf(ISpanContext spanContext)
        {
            _references.Add(SpanReference.ChildOf(spanContext));
            return this;
        }

        /// <summary>
        /// Adds a Span Reference with a ChildOfRef to parent span.
        /// If span == null, the option has no effect.
        /// </summary>
        /// <param name="span">Parent span</param>
        public SpanBuilder AsChildOf(ISpan span)
        {
            return AsChildOf(span.GetSpanContext());
        }

        /// <summary>
        /// Adds a Span Reference with a FollowsFromRef to parent span.
        /// If spanContext == null, the option has no effect.
        /// </summary>
        /// <param name="spanContext">Parent span context</param>
        public SpanBuilder FollowsFrom(ISpanContext spanContext)
        {
            _references.Add(SpanReference.FollowsFrom(spanContext));
            return this;
        }

        /// <summary>
        /// Adds a Span Reference with a FollowsFromRef to parent span.
        /// If span == null, the option has no effect.
        /// </summary>
        /// <param name="span">Parent span</param>
        public SpanBuilder FollowsFrom(ISpan span)
        {
            return AsChildOf(span.GetSpanContext());
        }

        /// <summary>
        /// Adds a string tag.
        /// </summary>
        public SpanBuilder WithTag(string key, string value)
        {
            _tags[key] = value; 
            return this;
        }

        /// <summary>
        /// Adds a number tag.
        /// </summary>
        public SpanBuilder WithTag(string key, int value)
        {
            _tags[key] = value.ToString();
            return this;
        }

        /// <summary>
        /// Adds a boolean tag.
        /// </summary>
        public SpanBuilder WithTag(string key, bool value)
        {
            _tags[key] = value.ToString();
            return this;
        }

        /// <summary>
        /// Sets the time when the span began.
        /// </summary>
        /// <param name="startTime">StartTime when the span began</param>
        public SpanBuilder WithStartTime(DateTime startTime)
        {
            _startTime = startTime;
            return this;
        }

        /// <summary>
        /// Start a new span based on the builder config.
        /// </summary>
        /// <returns>A new span.</returns>
        public ISpan Start()
        {
            return _tracer.StartSpan(_operationName,
                new StartSpanOptions()
                {
                    StartTime = _startTime ?? DateTime.Now,
                    Tag = _tags,
                    References = _references,
                });
        }
    }
}
