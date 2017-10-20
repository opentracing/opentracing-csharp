namespace OpenTracing.Propagation
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The default <see cref="ITextMap"/> implementation which wraps an arbitrary <see cref="IDictionary{TKey,TValue}"/>.
    /// </summary>
    public class DictionaryCarrier : ITextMap
    {
        private readonly IDictionary<string, string> _payload;

        public DictionaryCarrier(IDictionary<string, string> payload)
        {
            if (payload == null)
            {
                throw new ArgumentNullException(nameof(payload));
            }

            _payload = payload;
        }

        public IEnumerable<KeyValuePair<string, string>> GetEntries()
        {
            return _payload;
        }

        public string Get(string key)
        {
            string value;
            return _payload.TryGetValue(key, out value) ? value : null;
        }

        public void Set(string key, string value)
        {
            _payload[key] = value;
        }
    }
}