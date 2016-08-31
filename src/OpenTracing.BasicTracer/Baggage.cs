using System;
using System.Collections.Generic;

namespace OpenTracing.BasicTracer
{
    public class Baggage
    {
        private readonly IDictionary<string, string> _items = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        public void Set(string key, string value)
        {
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
    }
}