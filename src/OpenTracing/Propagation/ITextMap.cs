using System.Collections.Generic;

namespace OpenTracing.Propagation
{
    /// <summary>
    /// <see cref="ITextMap"/> is a built-in carrier for <see cref="ITracer.Inject"/> and <see cref="ITracer.Extract"/>. 
    /// ITextMap implementations allow Tracers to read and write key:value String pairs from arbitrary underlying sources of data.
    /// </summary>
    public interface ITextMap
    {
        /// <summary>
        /// <para>Returns all key:value pairs from the underlying source.</para>
        /// <para>Note that for some Formats, the iterator may include entries that
        /// were never injected by a Tracer implementation (e.g., unrelated HTTP headers).</para>
        /// </summary>
        IEnumerable<KeyValuePair<string, string>> GetEntries();

        /// <summary>
        /// Returns the key's value from the underlying source, or null if the key doesn't exist.
        /// </summary>
        /// <param name="key">The key for which a value should be returned.</param>
        string Get(string key);

        /// <summary>
        /// Adds a key:value pair into the underlying source. If the source already contains the key, the value will be overwritten.
        /// </summary>
        /// <param name="key">A string, possibly with constraints dictated by the particular Format this <see cref="ITextMap"/> is paired with.</param>
        /// <param name="value">A String, possibly with constraints dictated by the particular Format this <see cref="ITextMap"/> is paired with.</param>
        void Set(string key, string value);
    }
}