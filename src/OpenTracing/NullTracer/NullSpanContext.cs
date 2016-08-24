using System.Collections.Generic;

namespace OpenTracing.NullTracer
{
    public class NullSpanContext : ISpanContext
    {
        internal static readonly NullSpanContext Instance = new NullSpanContext();

        private readonly Dictionary<string, string> _baggage = new Dictionary<string, string>();

        private NullSpanContext()
        {
        }

        public IEnumerable<KeyValuePair<string, string>> GetBaggageItems()
        {
            return _baggage;
        }
    }
}