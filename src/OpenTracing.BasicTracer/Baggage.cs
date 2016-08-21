using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace OpenTracing.BasicTracer
{
    public class Baggage
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

        private readonly IDictionary<string, string> _items = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        public void Set(string key, string value)
        {
            if (!IsValidBaggageKey(key))
            {
                throw new ArgumentException("Invalid baggage key: '" + key + "'", nameof(key));
            }

            _items[key] = value;
        }

        public string Get(string key)
        {
            string value;
            return _items.TryGetValue(key, out value) ? value : null;
        }

        public IEnumerable<KeyValuePair<string, string>> GetAll()
        {
            return _items;
        }

        public void Merge(Baggage other)
        {
            Merge(other?.GetAll());
        }

        public void Merge(IEnumerable<KeyValuePair<string, string>> other)
        {
            if (other == null)
                return;

            // Copy entries into local dictionary instead of setting it directly
            // to make sure the case insensitive comparer is used.
            foreach (var kvp in other)
            {
                Set(kvp.Key, kvp.Value);
            }
        }

        private bool IsValidBaggageKey(string key)
        {

            return BaggageItemKeyFormat.IsMatch(key);
        }
    }
}