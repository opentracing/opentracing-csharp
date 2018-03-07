using System.Collections.Generic;
using System.Linq;

namespace OpenTracing.Mock
{
    /// <summary>
    /// <see cref="MockSpanContext"/> implements a Dapper-like <see cref="ISpanContext"/> with a trace-id and span-id.
    /// <para/>
    /// Note that parent ids are part of the <see cref="MockSpan"/>, not the <see cref="MockSpanContext"/>
    /// (since they do not need to propagate between processes).
    /// </summary>
    public sealed class MockSpanContext : ISpanContext
    {
        private readonly IDictionary<string, string> _baggage;

        public long TraceId { get; }

        public long SpanId { get; }

        /// <summary>
        /// An internal constructor to create a new <see cref="MockSpanContext"/>.
        /// This should only be called by <see cref="MockSpan"/> and/or <see cref="MockTracer"/>.
        /// </summary>
        /// <param name="traceId">The id of the trace.</param>
        /// <param name="spanId">The id of the span.</param>
        /// <param name="baggage">The MockContext takes ownership of the baggage parameter.</param>
        /// <seealso cref="MockSpanContext.WithBaggageItem(string, string)"/>
        internal MockSpanContext(long traceId, long spanId, IDictionary<string, string> baggage)
        {
            TraceId = traceId;
            SpanId = spanId;
            _baggage = baggage;
        }

        public IEnumerable<KeyValuePair<string, string>> GetBaggageItems()
        {
            return _baggage;
        }

        public string GetBaggageItem(string key)
        {
            if (_baggage.ContainsKey(key))
                return _baggage[key];

            return null;
        }

        /// <summary>
        /// Create and return a new (immutable) MockContext with the added baggage item.
        /// </summary>
        public MockSpanContext WithBaggageItem(string key, string val)
        {
            var newBaggage = new Dictionary<string, string>(_baggage);

            newBaggage[key] = val;

            return new MockSpanContext(TraceId, SpanId, newBaggage);
        }
    }
}
