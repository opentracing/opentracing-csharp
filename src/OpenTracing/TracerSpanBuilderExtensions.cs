using System;
using System.Collections.Generic;

namespace OpenTracing
{
    public static class TracerSpanBuilderExtensions
    {
        public static ISpanBuilder BuildSpan(ITracer tracer, string operationName)
        {
            return new SpanBuilder(tracer, operationName);
        }

        private sealed class SpanBuilder : ISpanBuilder
        {
            private readonly ITracer tracer;

            private string operationName;
            private ICollection<Tuple<string, ISpanContext>> references;
            private DateTimeOffset? explicitStartTime;
            private ICollection<KeyValuePair<string, string>> stringTags;
            private ICollection<KeyValuePair<string, bool>> boolTags;
            private ICollection<KeyValuePair<string, double>> doubleTags;
            private ICollection<KeyValuePair<string, int>> intTags;

            public SpanBuilder(ITracer tracer, string operationName)
            {
                this.tracer = tracer;
                this.operationName = operationName;
                this.references = new List<Tuple<string, ISpanContext>>();
                this.stringTags = new List<KeyValuePair<string, string>>();
                this.boolTags = new List<KeyValuePair<string, bool>>();
                this.doubleTags = new List<KeyValuePair<string, double>>();
                this.intTags = new List<KeyValuePair<string, int>>();
            }

            public ISpanBuilder AsChildOf(ISpan parent)
            {
                return this.AsChildOf(parent.Context);
            }

            public ISpanBuilder AsChildOf(ISpanContext parent)
            {
                return this.AddReference(References.ChildOf, parent);
            }

            public ISpanBuilder FollowsFrom(ISpan parent)
            {
                return this.FollowsFrom(parent.Context);
            }

            public ISpanBuilder FollowsFrom(ISpanContext parent)
            {
                return this.AddReference(References.FollowsFrom, parent);
            }

            public ISpanBuilder AddReference(string referenceType, ISpanContext referencedContext)
            {
                this.references.Add(Tuple.Create(referenceType, referencedContext));
                return this;
            }

            public ISpanBuilder WithTag(string key, bool value)
            {
                this.boolTags.Add(new KeyValuePair<string, bool>(key, value));
                return this;
            }

            public ISpanBuilder WithTag(string key, double value)
            {
                this.doubleTags.Add(new KeyValuePair<string, double>(key, value));
                return this;
            }

            public ISpanBuilder WithTag(string key, int value)
            {
                this.intTags.Add(new KeyValuePair<string, int>(key, value));
                return this;
            }

            public ISpanBuilder WithTag(string key, string value)
            {
                this.stringTags.Add(new KeyValuePair<string, string>(key, value));
                return this;
            }

            public ISpanBuilder WithStartTimestamp(DateTimeOffset startTimestamp)
            {
                this.explicitStartTime = startTimestamp;
                return this;
            }

            public ISpan Start()
            {
                return this.tracer.StartSpan(
                    this.operationName,
                    this.references,
                    this.explicitStartTime,
                    this.stringTags,
                    this.boolTags,
                    this.doubleTags,
                    this.intTags);
            }
        }
    }
}