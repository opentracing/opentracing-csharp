using System;
using System.Collections.Generic;

namespace OpenTracing.Propagation
{
    /// <summary>
    /// The default <see cref="ITextMapCarrier"/> implementation which wraps an arbitrary <see cref="IDictionary{TKey,TValue}"/>.
    /// </summary>
    public class TextMapCarrier : ITextMapCarrier
    {
        private readonly IDictionary<string, string> _payload;

        public TextMapCarrier(IDictionary<string, string> payload)
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

        public void Add(string key, string value)
        {
            _payload[key] = value;
        }
    }
}