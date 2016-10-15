using System;
using System.Collections;
using System.Collections.Generic;

namespace OpenTracing
{
    /// <summary>
    /// <para>Represents a collection of key:value pairs for a <see cref="ISpan.Log"/> call.</para>
    /// <para>In contrast to an arbitrary dictionary, this type does not do unique key validations.</para>
    /// </summary>
    public class Fields : IEnumerable<KeyValuePair<string, object>>
    {
        private readonly List<KeyValuePair<string, object>> _fields = new List<KeyValuePair<string, object>>();

        /// <summary>
        /// Adds the given key:value pair to the collection.
        /// </summary>
        public void Add(string key, object value)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            _fields.Add(new KeyValuePair<string, object>(key, value));
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator() => _fields.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Shorthand for <code>new Fields { { key, value } }</code>
        /// </summary>
        public static IEnumerable<KeyValuePair<string, object>> Create(string key, object value)
        {
            return new Fields { { key, value} };
        }

        /// <summary>
        /// Shorthand for <c>new Fields { { key1, value1 }, { key2, value2 } }</c>
        /// </summary>
        public static IEnumerable<KeyValuePair<string, object>> Create(
            string key1, object value1,
            string key2, object value2)
        {
            return new Fields { { key1, value1}, { key2, value2} };
        }

        /// <summary>
        /// Shorthand for <c>new Fields { { key1, value1 }, ..., { key3, value3 } }</c>
        /// </summary>
        public static IEnumerable<KeyValuePair<string, object>> Create(
            string key1, object value1,
            string key2, object value2,
            string key3, object value3)
        {
            return new Fields { { key1, value1}, { key2, value2}, { key3, value3 } };
        }

        /// <summary>
        /// Shorthand for <c>new Fields { { key1, value1 }, ..., { key4, value4 } }</c>
        /// </summary>
        public static IEnumerable<KeyValuePair<string, object>> Create(
            string key1, object value1,
            string key2, object value2,
            string key3, object value3,
            string key4, object value4)
        {
            return new Fields { { key1, value1}, { key2, value2}, { key3, value3 }, { key4, value4 } };
        }

        /// <summary>
        /// Shorthand for <c>new Fields { { key1, value1 }, ..., { key5, value5 } }</c>
        /// </summary>
        public static IEnumerable<KeyValuePair<string, object>> Create(
            string key1, object value1,
            string key2, object value2,
            string key3, object value3,
            string key4, object value4,
            string key5, object value5)
        {
            return new Fields { { key1, value1}, { key2, value2}, { key3, value3 }, { key4, value4 }, { key5, value5 } };
        }
    }
}