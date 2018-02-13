using System;
using System.Collections;
using System.Collections.Generic;

namespace OpenTracing.Propagation
{
    /// <summary>
    /// A ITextMap carrier for use with Tracer.extract() ONLY (it has no mutating methods). Note that the ITextMap
    /// interface can be made to wrap around arbitrary data types (not just Dictionary{string, string} as illustrated here).
    /// </summary>
    /// <seealso cref="ITracer.Extract{TCarrier}"/>
    public sealed class TextMapExtractAdapter : ITextMap
    {
        private readonly IDictionary<string, string> _dictionary;

        public TextMapExtractAdapter(IDictionary<string, string> dictionary)
        {
            _dictionary = dictionary;
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        public void Set(string key, string value)
        {
            throw new InvalidOperationException(
                $"{nameof(TextMapExtractAdapter)} should only be used with {nameof(ITracer)}.{nameof(ITracer.Extract)}");
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
