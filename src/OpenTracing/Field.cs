using System;

namespace OpenTracing
{
    /// <summary>
    /// <para><see cref="Field"/> represents a single key:value pair in a <see cref="ISpan.Log"/> record.</para>
    /// <para>All Tracer implementations must support bool, numeric, and String values;
    /// some may also support arbitrary Object values.</para>
    /// </summary>
    public struct Field
    {
        public string Key { get; }
        public object Value { get; }

        /// <summary>
        /// Creates a new <see cref="Field"/>.
        /// </summary>
        /// <param name="key">The key of a <see cref="ISpan.Log"/> record.</param>
        /// <param name="value">The value of a <see cref="ISpan.Log"/> record.</param>
        public Field(string key, object value)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            Key = key;
            Value = value;
        }

        /// <summary>
        /// Creates a new <see cref="Field"/> with the given <paramref name="key"/> and <paramref name="value"/>.
        /// </summary>
        /// <param name="key">The key of a <see cref="ISpan.Log"/> record.</param>
        /// <param name="value">The value of a <see cref="ISpan.Log"/> record.</param>
        public static Field Of<TValue>(string key, TValue value)
        {
            return new Field(key, value);
        }
    }
}
