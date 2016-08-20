using System.Collections.Generic;

namespace OpenTracing.NullTracer
{
    public class NullSpanContext : ISpanContext
    {
        private readonly IDictionary<string, string> _items = new Dictionary<string, string>();

        public IDictionary<string, string> BaggageItems => _items;
    }
}