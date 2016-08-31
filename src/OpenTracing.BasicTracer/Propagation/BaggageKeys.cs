namespace OpenTracing.BasicTracer.Propagation
{
    public static class BaggageKeys
    {
        public const string BaggagePrefix = "ot-bg-";

        public const string TraceId = "ot-traceid";
        public const string SpanId = "ot-spanid";
        public const string Sampled = "ot-sampled";
    }
}