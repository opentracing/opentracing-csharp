namespace OpenTracing.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using OpenTracing.Propagation;
    using Xunit;

    public class DictionaryCarrierTests
    {
        private IDictionary<string, string> GetDictionary(int items)
        {
            var data = new Dictionary<string, string>();

            for (int i = 1; i <= items; i++)
            {
                data.Add($"key{i}", $"value{i}");
            }

            return data;
        }

        [Fact]
        public void Ctor_throws_if_data_missing()
        {
            IDictionary<string, string> data = null;

            Assert.Throws<ArgumentNullException>(() => new DictionaryCarrier(data));
        }

        [Fact]
        public void GetEntries_returns_all_entries()
        {
            var data = new Dictionary<string, string>
            {
                { "key1", "value1" },
                { "key2", "value2" },
                { "key3", "value3" }
            };
            var carrier = new DictionaryCarrier(data);

            var resultEntries = carrier.GetEntries();

            Assert.True(resultEntries.SequenceEqual(data));
        }

        [Fact]
        public void Get_throws_if_key_missing()
        {
            var data = new Dictionary<string, string>
            {
                { "key1", "value1" }
            };
            var carrier = new DictionaryCarrier(data);

            Assert.Throws<ArgumentNullException>(() => carrier.Get(null));
        }

        [Fact]
        public void Get_returns_null_if_key_is_empty()
        {
            var data = new Dictionary<string, string>
            {
                { "key1", "value1" }
            };
            var carrier = new DictionaryCarrier(data);

            Assert.Null(carrier.Get(""));
        }

        [Fact]
        public void Get_returns_null_if_key_is_not_found()
        {
            var data = new Dictionary<string, string>
            {
                { "key1", "value1" }
            };
            var carrier = new DictionaryCarrier(data);

            Assert.Null(carrier.Get("invalid"));
        }

        [Fact]
        public void Get_returns_correct_value()
        {
            var data = new Dictionary<string, string>
            {
                { "key1", "value1" },
                { "key2", "value2" },
                { "key3", "value3" }
            };
            var carrier = new DictionaryCarrier(data);

            var result = carrier.Get("key2");

            Assert.Equal("value2", result);
        }

        [Fact]
        public void Set_throws_if_key_missing()
        {
            var data = new Dictionary<string, string> { { "key1", "value1" } };
            var carrier = new DictionaryCarrier(data);

            Assert.Throws<ArgumentNullException>(() => carrier.Set(null, "value"));
        }

        [Fact]
        public void Set_succeeds_if_value_null()
        {
            var data = new Dictionary<string, string> { { "key1", "value1" } };
            var carrier = new DictionaryCarrier(data);

            carrier.Set("key2", null);

            Assert.Equal(2, data.Count);
        }

        [Fact]
        public void Set_succeeds_if_new_key()
        {
            var data = new Dictionary<string, string> { { "key1", "value1" } };
            var carrier = new DictionaryCarrier(data);

            carrier.Set("key2", "value2");

            Assert.Equal(2, data.Count);
        }

        [Fact]
        public void Set_overwrites_existing_key()
        {
            var data = new Dictionary<string, string> { { "key1", "value1" } };
            var carrier = new DictionaryCarrier(data);

            carrier.Set("key1", "value2");

            Assert.Equal(1, data.Count);
            Assert.Equal("value2", data["key1"]);
        }
    }
}