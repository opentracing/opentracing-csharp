using System;
using System.Collections;
using System.Collections.Generic;

namespace OpenTracing.Propagation
{
    /// <summary>
    /// A TextMap carrier for use with <see cref="ITracer.Inject{TCarrier}"/> ONLY (it has no read methods). Note that
    /// the TextMap interface can be made to wrap around arbitrary data types (not just Dictionary{string, string} as
    /// illustrated here).
    /// </summary>
    /// <seealso cref="ITracer.Inject{TCarrier}"/>
    public sealed class TextMapInjectAdapter : TextMap
    {
        private readonly ICollection<KeyValuePair<string, string>> _keyValuePairs;

        public TextMapInjectAdapter(ICollection<KeyValuePair<string, string>> keyValuePairs)
        {
            _keyValuePairs = keyValuePairs;
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            throw new InvalidOperationException(
                $"{nameof(TextMapExtractAdapter)} should only be used with {nameof(ITracer)}.{nameof(ITracer.Extract)}");
        }

        public void Set(string key, string value)
        {
            _keyValuePairs.Add(new KeyValuePair<string, string>(key, value));
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}