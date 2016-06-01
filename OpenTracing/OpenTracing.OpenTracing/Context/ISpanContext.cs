using System.Collections.Generic;

namespace OpenTracing.Context
{
    public interface ISpanContext
    {
        IReadOnlyDictionary<string, string> GetBaggageItems();
        void SetBaggageItem(string restrictedKey, string value);
    }
}
