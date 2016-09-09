using System;

namespace OpenTracing
{
    /// <summary>
    /// <para><see cref="LogField"/> represents a single key:value pair in a <see cref="ISpan.Log"/> record.</para>
    /// <para>All Tracer implementations must support bool, numeric, and String values;
    /// some may also support arbitrary Object values.</para>
    /// </summary>
    public struct LogField
    {
        public string Key { get; }
        public object Value { get; }

        /// <summary>
        /// Creates a new <see cref="LogField"/>.
        /// </summary>
        /// <param name="key">The key of a <see cref="ISpan.Log"/> record.</param>
        /// <param name="value">The value of a <see cref="ISpan.Log"/> record.</param>
        public LogField(string key, object value)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            Key = key;
            Value = value;
        }
    }
}
