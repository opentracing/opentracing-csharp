using System;

namespace OpenTracing.NullTracer
{
    public class NullSpan : ISpan
    {
        internal static readonly NullSpan Instance = new NullSpan(NullSpanContext.Instance);

        public ISpanContext Context { get; }

        private NullSpan(ISpanContext context)
        {
            Context = context;
        }

        public ISpan SetTag(string key, object value)
        {
            return this;
        }

        public ISpan Log(string eventName, object payload = null)
        {
            return this;
        }

        public ISpan Log(DateTimeOffset timestamp, string eventName, object payload = null)
        {
            return this;
        }

        public void Finish(FinishSpanOptions options = null)
        {
        }
    }
}