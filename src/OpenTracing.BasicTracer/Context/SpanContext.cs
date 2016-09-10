using System.Collections.Generic;

namespace OpenTracing.BasicTracer.Context
{
    public class SpanContext : ISpanContext
    {
        private readonly Baggage _baggage = new Baggage();

        public ulong TraceId { get; private set; }
        public ulong ParentId { get; private set; }
        public ulong SpanId { get; private set; }

        public bool Sampled { get; private set; }

        public SpanContext(ulong traceId, ulong parentId, ulong spanId, bool sampled, Baggage baggage)
        {
            TraceId = traceId;
            ParentId = parentId;
            SpanId = spanId;
            Sampled = sampled;

            _baggage.Merge(baggage);
        }

        public IEnumerable<KeyValuePair<string, string>> GetBaggageItems()
        {
            return _baggage.GetAll();
        }

        public string GetBaggageItem(string key)
        {
            return _baggage.Get(key);
        }

        public ISpanContext SetBaggageItem(string key, string value)
        {
            _baggage.Set(key, value);
            return this;
        }
    }
}
