using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;

namespace OpenTracing.BasicTracer
{
    public class SpanContext : ISpanContext
    {
        /// <summary>
        /// <para>Baggage keys have a restricted format: implementations may wish to use them
        /// as HTTP header keys (or key suffixes) and of course HTTP headers are case insensitive.</para>
        /// <para>As such, Baggage keys MUST match the regular expression (?i:[a-z0-9][-a-z0-9]*),
        /// and – per the ?i: – they are case-insensitive.</para>
        /// <para>That is, the Baggage key must start with a letter or number,
        /// and the remaining characters must be letters, numbers, or hyphens.</para>
        /// </summary>
        private static readonly Regex BaggageItemKeyFormat = new Regex(@"^(?i:[a-z0-9][-a-z0-9]*)$");

        private readonly IDictionary<string, string> _baggageItems = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        public Guid TraceId { get; }
        public Guid SpanId { get; }
        public bool Sampled { get; }


        public SpanContext(Guid traceId, Guid spanId, bool sampled, IDictionary<string, string> baggageItems = null)
        {
            TraceId = traceId;
            SpanId = spanId;
            Sampled = sampled;

            if (baggageItems != null)
            {
                // Copy entries into local dictionary instead of setting it directly
                // to make sure the case insensitive comparer is used.
                foreach (var kvp in baggageItems)
                {
                    _baggageItems[kvp.Key] = kvp.Value;
                }
            }
        }

        public IEnumerable<KeyValuePair<string, string>> GetBaggageItems()
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
            if (!IsValidBaggageKey(key))
            {
                throw new ArgumentException("Invalid baggage key: '" + key + "'", nameof(key));
            }

            _baggageItems[key] = value;
        }

        private bool IsValidBaggageKey(string key)
        {

            return BaggageItemKeyFormat.IsMatch(key);
        }

    }
}
