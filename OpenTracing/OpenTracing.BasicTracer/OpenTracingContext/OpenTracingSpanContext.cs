using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace OpenTracing.BasicTracer.OpenTracingContext
{
    public class OpenTracingSpanContext : Context.ISpanContext
    {
        public OpenTracingSpanContext(ulong traceId, ulong parentId, ulong spanId, bool sampled, Dictionary<string, string> baggage)
        {
            TraceId = traceId;
            ParentId = parentId;
            SpanId = spanId;
            Sampled = sampled;

            _baggage = baggage;
        }

        public ulong TraceId { get; private set; }
        public ulong ParentId { get; private set; }
        public ulong SpanId { get; private set; }
        public bool Sampled { get; private set; }

        private Dictionary<string, string> _baggage { get; set; }
        public IReadOnlyDictionary<string, string> Baggage { get { return new ReadOnlyDictionary<string, string>(_baggage); } }

        public IReadOnlyDictionary<string, string> GetBaggageItems()
        {
            return _baggage;
        }

        public void SetBaggageItem(string key, string value)
        {
            _baggage.Add(key, value);
        }
    }
}
