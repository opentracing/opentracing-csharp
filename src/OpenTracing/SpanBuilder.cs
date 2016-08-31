using System;
using System.Collections.Generic;

namespace OpenTracing
{
    /// <summary>
    /// Helper builder for convenience to construct startoptions and create a new span.
    /// </summary>
    public class SpanBuilder
    {
        // TODO doesn't have the typed tag extension methods.

        private readonly ITracer _tracer;
        private readonly string _operationName;

        private readonly List<SpanReference> _references = new List<SpanReference>();
        private readonly IDictionary<string, object> _tags = new Dictionary<string, object>();
        
        private DateTimeOffset? _startTimestamp;

        public SpanBuilder(ITracer tracer, string operationName)
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

        /// <summary>
        /// Adds a Span Reference with a ChildOfRef to parent span.
        /// If spanContext == null, the option has no effect.
        /// </summary>
        /// <param name="spanContext">Parent span context</param>
        public SpanBuilder AsChildOf(ISpanContext spanContext)
        {
            return AddReference(SpanReference.ChildOf(spanContext));
        }

        /// <summary>
        /// Adds a Span Reference with a ChildOfRef to parent span.
        /// If span == null, the option has no effect.
        /// </summary>
        /// <param name="span">Parent span</param>
        public SpanBuilder AsChildOf(ISpan span)
        {
            return AsChildOf(span?.Context);
        }

        /// <summary>
        /// Adds a Span Reference with a FollowsFromRef to parent span.
        /// If spanContext == null, the option has no effect.
        /// </summary>
        /// <param name="spanContext">Parent span context</param>
        public SpanBuilder FollowsFrom(ISpanContext spanContext)
        {
            return AddReference(SpanReference.FollowsFrom(spanContext));
        }

        /// <summary>
        /// Adds a Span Reference with a FollowsFromRef to parent span.
        /// If span == null, the option has no effect.
        /// </summary>
        /// <param name="span">Parent span</param>
        public SpanBuilder FollowsFrom(ISpan span)
        {
            return FollowsFrom(span?.Context);
        }

        /// <summary>
        /// Adds a reference to the new Span.
        /// If the <paramref name="reference"/> is null, the option has no effect.
        /// </summary>
        /// <param name="reference">The reference that should be added to the new Span.</param>
        /// <returns>The current instance for chaining.</returns>
        public SpanBuilder AddReference(SpanReference reference)
        {
            if (reference != null)
            {
                _references.Add(reference);
            }

            return this;
        }

        /// <summary>
        /// Adds a tag.
        /// </summary>
        public SpanBuilder WithTag(string key, object value)
        {
            _tags[key] = value;
            return this;
        }

        /// <summary>
        /// Sets the time when the span began.
        /// </summary>
        /// <param name="startTimestamp">StartTime when the span began</param>
        public SpanBuilder WithStartTime(DateTimeOffset startTimestamp)
        {
            _startTimestamp = startTimestamp;
            return this;
        }

        /// <summary>
        /// Start a new span based on the builder config.
        /// </summary>
        /// <returns>A new span.</returns>
        public ISpan Start()
        {
            return _tracer.StartSpan(_operationName,
                new StartSpanOptions
                {
                    StartTimestamp = _startTimestamp ?? DateTimeOffset.UtcNow,
                    Tags = _tags,
                    References = _references,
                });
        }
    }
}