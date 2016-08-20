namespace OpenTracing.NullTracer
{
    public class NullSpanContext : ISpanContext
    {
        internal static readonly NullSpanContext Instance = new NullSpanContext();

        private NullSpanContext()
        {
        }

        public string GetBaggageItem(string key)
        {
            return null;
        }

        public void SetBaggageItem(string key, string value)
        {
        }
    }
}