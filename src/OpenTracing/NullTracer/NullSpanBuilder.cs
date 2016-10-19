using System;

namespace OpenTracing.NullTracer
{
    public class NullSpanBuilder : ISpanBuilder
    {
        public static readonly NullSpanBuilder Instance = new NullSpanBuilder();

        private NullSpanBuilder()
        {
        }

        public ISpanBuilder AsChildOf(ISpan parent)
        {
            return this;
        }

        public ISpanBuilder AsChildOf(ISpanContext parent)
        {
            return this;
        }

        public ISpanBuilder FollowsFrom(ISpan parent)
        {
            return this;
        }

        public ISpanBuilder FollowsFrom(ISpanContext parent)
        {
            return this;
        }

        public ISpanBuilder AddReference(string referenceType, ISpanContext referencedContext)
        {
            return this;
        }

        public ISpanBuilder WithStartTimestamp(DateTime startTimestamp)
        {
            return this;
        }

        public ISpanBuilder WithTag(string key, bool value)
        {
            return this;
        }

        public ISpanBuilder WithTag(string key, double value)
        {
            return this;
        }

        public ISpanBuilder WithTag(string key, int value)
        {
            return this;
        }

        public ISpanBuilder WithTag(string key, string value)
        {
            return this;
        }

        public ISpan Start()
        {
            return NullSpan.Instance;
        }
    }
}