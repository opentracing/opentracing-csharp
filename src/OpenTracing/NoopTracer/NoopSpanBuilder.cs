namespace OpenTracing.NoopTracer
{
    using System;

    internal sealed class NoopSpanBuilder : ISpanBuilder
    {
        public static ISpanBuilder Instance = new NoopSpanBuilder();

        private NoopSpanBuilder()
        {
        }

        public ISpanBuilder SetOperationName(string operationName)
        {
            return this;
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

        public ISpanBuilder SetTag(string key, bool value)
        {
            return this;
        }

        public ISpanBuilder SetTag(string key, double value)
        {
            return this;
        }

        public ISpanBuilder SetTag(string key, int value)
        {
            return this;
        }

        public ISpanBuilder SetTag(string key, string value)
        {
            return this;
        }

        public ISpanBuilder SetStartTimestamp(DateTimeOffset startTimestamp)
        {
            return this;
        }

        public ISpan Start()
        {
            return NoopSpan.Instance;
        }
    }
}