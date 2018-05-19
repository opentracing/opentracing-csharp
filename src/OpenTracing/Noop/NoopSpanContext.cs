using System.Collections.Generic;
using System.Linq;

namespace OpenTracing.Noop
{
    public sealed class NoopSpanContext : ISpanContext
    {
        internal static readonly NoopSpanContext Instance = new NoopSpanContext();

        private NoopSpanContext()
        {
        }

        public string TraceId { get; }

        public string SpanId { get; }

        public IEnumerable<KeyValuePair<string, string>> GetBaggageItems()
        {
            return Enumerable.Empty<KeyValuePair<string, string>>();
        }

        public override string ToString()
        {
            return nameof(NoopSpanContext);
        }
    }
}
