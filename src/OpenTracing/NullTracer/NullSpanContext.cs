namespace OpenTracing.NullTracer
{
    using System.Collections.Generic;

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