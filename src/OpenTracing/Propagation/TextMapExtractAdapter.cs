using System;
using System.Collections;
using System.Collections.Generic;

namespace OpenTracing.Propagation
{
    /// <summary>
    /// A TextMap carrier for use with Tracer.extract() ONLY (it has no mutating methods). Note that the TextMap
    /// interface can be made to wrap around arbitrary data types (not just Dictionary{string, string} as illustrated here).
    /// </summary>
    /// <seealso cref="ITracer.Extract{T}"/>
    public sealed class TextMapExtractAdapter : TextMap
    {
        private readonly IDictionary<string, string> dictionary;

        public TextMapExtractAdapter(IDictionary<string, string> dictionary)
        {
            this.dictionary = dictionary;
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return dictionary.GetEnumerator();
        }

        public void Put(string key, string value)
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