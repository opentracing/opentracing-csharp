namespace OpenTracing.NoopTracer
{
    using System.Collections.Generic;

    internal sealed class NoopSpanContext : ISpanContext
    {
        public static ISpanContext Instance = new NoopSpanContext();

        private NoopSpanContext()
        {
        }

        public IEnumerable<KeyValuePair<string, string>> GetBaggageItems()
        {
            return new Dictionary<string, string>();
        }
    }
}