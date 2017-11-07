using System;
using System.Collections.Generic;

namespace OpenTracing
{
    using System.Linq;

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
            private IDictionary<string, object> tags;

            public SpanBuilder(ITracer tracer, string operationName)
            {
                this.tracer = tracer;
                this.operationName = operationName;
                this.references = new List<Tuple<string, ISpanContext>>();
                this.tags = new Dictionary<string, object>();
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
                this.tags.Add(key, value);
                return this;
            }

            public ISpanBuilder WithTag(string key, double value)
            {
                this.tags.Add(key, value);
                return this;
            }

            public ISpanBuilder WithTag(string key, int value)
            {
                this.tags.Add(key, value);
                return this;
            }

            public ISpanBuilder WithTag(string key, string value)
            {
                this.tags.Add(key, value);
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
                    this.tags.Where(kvp => kvp.Value is string).Select(kvp => new KeyValuePair<string, string>(kvp.Key, (string) kvp.Value)),
                    this.tags.Where(kvp => kvp.Value is bool).Select(kvp => new KeyValuePair<string, bool>(kvp.Key, (bool)kvp.Value)),
                    this.tags.Where(kvp => kvp.Value is double).Select(kvp => new KeyValuePair<string, double>(kvp.Key, (double)kvp.Value)),
                    this.tags.Where(kvp => kvp.Value is int).Select(kvp => new KeyValuePair<string, int>(kvp.Key, (int)kvp.Value)));
            }
        }
    }
}