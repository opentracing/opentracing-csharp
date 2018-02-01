using System;
using System.Collections;
using System.Collections.Generic;

namespace OpenTracing.Propagation
{
    /// <summary>
    /// A TextMap carrier for use with <see cref="ITracer.Inject{T}"/> ONLY (it has no read methods).
    /// Note that the TextMap interface can be made to wrap around arbitrary data types (not just Dictionary{string,
    /// string}
    /// as illustrated here).
    /// </summary>
    /// <seealso cref="ITracer.Inject{T}"/>
    public sealed class TextMapInjectAdapter : TextMap
    {
        private readonly IDictionary<string, string> dictionary;

        public TextMapInjectAdapter(IDictionary<string, string> dictionary)
        {
            this.dictionary = dictionary;
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            throw new InvalidOperationException(
                $"{nameof(TextMapExtractAdapter)} should only be used with {nameof(ITracer)}.{nameof(ITracer.Extract)}");
        }

        public void Put(string key, string value)
        {
            dictionary.Add(key, value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}