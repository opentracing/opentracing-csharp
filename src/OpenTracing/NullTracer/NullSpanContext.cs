using System.Collections.Generic;

namespace OpenTracing.NullTracer
{
    public class NullSpanContext : ISpanContext
    {
        internal static readonly NullSpanContext Instance = new NullSpanContext();

        private readonly IDictionary<string, string> _baggage = new Dictionary<string, string>();

        private NullSpanContext()
        {
        }

        public string GetBaggageItem(string key)
        {
            return null;
        }

        public IEnumerable<KeyValuePair<string, string>> GetBaggageItems()
        {
            return _baggage;
        }

        public void SetBaggageItem(string key, string value)
        {
        }
    }
}