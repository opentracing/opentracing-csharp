using System.Collections.Generic;

namespace OpenTracing.Propagation
{
    /// <summary>
    /// <see cref="TextMap"/> is a built-in carrier for <see cref="ITracer.Inject{T}"/> and
    /// <see cref="ITracer.Extract{T}"/>. TextMap implementations allows Tracers to read and write key:value String pairs from
    /// arbitrary underlying sources of data.
    /// </summary>
    /// <seealso cref="ITracer.Inject{T}"/>
    /// <seealso cref="ITracer.Extract{T}"/>
    public interface TextMap : IEnumerable<KeyValuePair<string, string>>
    {
        /// <summary>Puts a key:value pair into the TextMapWriter's backing store.</summary>
        /// <param name="key">A string, possibly with constraints dictated by the particular Format this TextMap is paired with</param>
        /// <param name="value">A string, possibly with constraints dictated by the particular Format this TextMap is paired with</param>
        /// <seealso cref="ITracer.Inject{T}"/>
        /// <seealso cref="BuiltinFormats.TextMap"/>
        /// <seealso cref="BuiltinFormats.HttpHeaders"/>
        void Set(string key, string value);
    }
}