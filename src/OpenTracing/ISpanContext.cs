using System.Collections.Generic;

namespace OpenTracing
{
    public interface ISpanContext
    {
        string GetBaggageItem(string key);

        IEnumerable<KeyValuePair<string, string>> GetBaggageItems();

        void SetBaggageItem(string key, string value);
    }
}