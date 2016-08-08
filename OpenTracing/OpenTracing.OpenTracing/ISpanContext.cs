using System.Collections.Generic;

namespace OpenTracing
{
    public interface ISpanContext
    {
        IReadOnlyDictionary<string, string> GetBaggageItems();
    }
}
