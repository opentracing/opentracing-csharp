using System;
using System.Collections;
using System.Collections.Generic;

namespace OpenTracing.Propagation
{
    /// <summary>
    /// A <see cref="ITextMap"/> carrier for use with <see cref="ITracer.Extract"/> ONLY (it has no mutating methods). Note that the
    /// <see cref="ITextMap"/> interface can be made to wrap around arbitrary data types
    /// (not just <code>Dictionary&lt;string, string&gt;</code> as illustrated here).
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
            throw new NotSupportedException(
                $"{nameof(TextMapExtractAdapter)} should only be used with {nameof(ITracer)}.{nameof(ITracer.Extract)}");
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
