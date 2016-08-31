using System;
using OpenTracing.Propagation;

namespace OpenTracing.BasicTracer.Propagation
{
    public class TextMapCarrierHandler
    {
        public void MapContextToCarrier(SpanContext context, ITextMap carrier)
        {
            carrier.Add(BaggageKeys.TraceId, context.TraceId.ToString());
            carrier.Add(BaggageKeys.SpanId, context.SpanId.ToString());
            carrier.Add(BaggageKeys.Sampled, context.Sampled.ToString());

            foreach (var kvp in context.GetBaggageItems())
            {
                carrier.Add(BaggageKeys.BaggagePrefix + kvp.Key, kvp.Value);
            }
        }

        public SpanContext MapCarrierToContext(ITextMap carrier)
        {
            // we can't create a reference without a trace-id
            Guid? traceId = TryGetGuid(carrier, BaggageKeys.TraceId);
            if (!traceId.HasValue)
                return null;
            
            // something is seriously wrong if we have a trace-id but no span-id
            Guid? spanId = TryGetGuid(carrier, BaggageKeys.SpanId);
            if (!spanId.HasValue)
                return null;

            bool sampled;
            bool.TryParse(carrier.Get(BaggageKeys.Sampled), out sampled);
            
            var baggage = new Baggage();

            foreach (var kvp in carrier.GetEntries())
            {
                if (kvp.Key.StartsWith(BaggageKeys.BaggagePrefix, StringComparison.OrdinalIgnoreCase))
                {
                    var key = kvp.Key.Substring(BaggageKeys.BaggagePrefix.Length);
                    baggage.Set(key, kvp.Value);
                }
            }

            return new SpanContext(traceId.Value, spanId.Value, sampled, baggage);
        }

        private Guid? TryGetGuid(ITextMap carrier, string key)
        {
            string strValue = carrier.Get(key);
            
            Guid guidValue;
            if (!Guid.TryParse(strValue, out guidValue))
                return null;

            if (guidValue == Guid.Empty)
                return null;

            return guidValue;
        }
    }
}