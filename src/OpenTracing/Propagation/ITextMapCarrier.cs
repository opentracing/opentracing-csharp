using System.Collections.Generic;

namespace OpenTracing.Propagation
{
    /// <summary>
    /// <see cref="ITextMapCarrier"/> is a built-in carrier for <see cref="ITracer.Inject"/> and <see cref="ITracer.Extract"/>. 
    /// TextMap implementations allow Tracers to read and write key:value String pairs from arbitrary underlying sources of data.
    /// </summary>
    public interface ITextMapCarrier : IInjectCarrier, IExtractCarrier
    {
        /// <summary>
        /// Returns all key:value pairs from the underlying source.
        /// </summary>
        IEnumerable<KeyValuePair<string, string>> GetEntries();

        /// <summary>
        /// Returns the key's value from the underlying source, or null if the key doesn't exist.
        /// </summary>
        /// <param name="key">The key for which a value should be returned.</param>
        /// <returns></returns>
        string Get(string key);

        /// <summary>
        /// Adds a key:value pair into the underlying source.
        /// </summary>
        void Add(string key, string value);
    }
}