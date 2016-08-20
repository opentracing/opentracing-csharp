using System.Collections.Generic;

namespace OpenTracing
{
    public interface ISpanContext
    {
        IDictionary<string, string> BaggageItems { get; }
    }
}