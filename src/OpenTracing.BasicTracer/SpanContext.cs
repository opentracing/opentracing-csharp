using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;

namespace OpenTracing.BasicTracer
{
    public class SpanContext : ISpanContext
    {
        private static readonly Regex BaggageItemKeyFormat = new Regex(@"^(?i:[a-z0-9][-a-z0-9]*)$");

        private readonly IDictionary<string, string> _baggageItems = new Dictionary<string, string>();

        public Guid TraceId { get; }
        public Guid? ParentId { get; }
        public Guid SpanId { get; }
        public bool Sampled { get; }


        public SpanContext(Guid traceId, Guid? parentId, Guid spanId, bool sampled, IDictionary<string, string> baggageItems = null)
        {
            TraceId = traceId;
            ParentId = parentId;
            SpanId = spanId;
            Sampled = sampled;

            _baggageItems = baggageItems ?? new Dictionary<string, string>();
        }

        public IDictionary<string, string> GetBaggageItems()
        {
            return _baggageItems;
        }

        public string GetBaggageItem(string key)
        {
            string value;
            if (_baggageItems.TryGetValue(key, out value))
            {
                return value;
            }

            return null;
        }

        public void SetBaggageItem(string key, string value)
        {
            if (!IsValidBaggaeKey(key))
            {
                throw new ArgumentException("Invalid baggage key: '" + key + "'", nameof(key));
            }

            _baggageItems[key] = value;
        }

        private bool IsValidBaggaeKey(string key)
        {

            return BaggageItemKeyFormat.IsMatch(key);
        }

    }
}
