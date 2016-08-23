using System.Collections.Generic;

namespace OpenTracing.BasicTracer.Context
{
    public interface ISpanContext : OpenTracing.ISpanContext
    {
        void SetBaggageItem(string restrictedKey, string value);
    }
}
