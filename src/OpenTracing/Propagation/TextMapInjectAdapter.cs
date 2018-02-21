using System;
using System.Collections;
using System.Collections.Generic;

namespace OpenTracing.Propagation
{
    /// <summary>
    /// A TextMap carrier for use with <see cref="ITracer.Inject{TCarrier}"/> ONLY (it has no read methods). Note that
    /// the ITextMap interface can be made to wrap around arbitrary data types (not just Dictionary{string, string} as
    /// illustrated here).
    /// </summary>
    /// <seealso cref="ITracer.Inject{TCarrier}"/>
    public sealed class TextMapInjectAdapter : ITextMap
    {
        private readonly IDictionary<string, string> _dictionary;

        public TextMapInjectAdapter(IDictionary<string, string> dictionary)
        {
            _dictionary = dictionary;
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            throw new NotSupportedException(
                $"{nameof(TextMapInjectAdapter)} should only be used with {nameof(ITracer)}.{nameof(ITracer.Inject)}");
        }

        public void Set(string key, string value)
        {
            _dictionary[key] = value;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
