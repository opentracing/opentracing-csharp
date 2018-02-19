using System;

namespace OpenTracing.Noop
{
    internal sealed class NoopSpanBuilder : ISpanBuilder
    {
        internal static NoopSpanBuilder Instance = new NoopSpanBuilder();

        private NoopSpanBuilder()
        {
        }

        public ISpanBuilder AddReference(string referenceType, ISpanContext referencedContext)
        {
            return this;
        }

        public ISpanBuilder AsChildOf(ISpanContext parent)
        {
            return this;
        }

        public ISpanBuilder AsChildOf(ISpan parent)
        {
            return this;
        }

        public ISpanBuilder IgnoreActiveSpan()
        {
            return this;
        }

        public ISpanBuilder WithTag(string key, string value)
        {
            return this;
        }

        public ISpanBuilder WithTag(string key, bool value)
        {
            return this;
        }

        public ISpanBuilder WithTag(string key, int value)
        {
            return this;
        }

        public ISpanBuilder WithTag(string key, double value)
        {
            return this;
        }

        public ISpanBuilder WithStartTimestamp(DateTimeOffset timestamp)
        {
            return this;
        }

        public IScope StartActive(bool finishSpanOnDispose)
        {
            return NoopScopeManager.NoopScope.Instance;
        }

        public ISpan Start()
        {
            return NoopSpan.Instance;
        }

        public override string ToString()
        {
            return nameof(NoopSpanBuilder);
        }
    }
}
