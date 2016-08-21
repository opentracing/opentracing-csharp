using System;
using OpenTracing.Propagation;

namespace OpenTracing.BasicTracer.Propagation
{
    public class TextMapCarrierHandler :
        IInjectCarrierHandler<TextMapCarrier>,
        IExtractCarrierHandler<TextMapCarrier>
    {
        public void MapContextToCarrier(SpanContext context, TextMapCarrier carrier)
        {
            carrier.TextMap[BaggageKeys.TraceId] = context.TraceId.ToString();
            carrier.TextMap[BaggageKeys.SpanId] = context.SpanId.ToString();
            carrier.TextMap[BaggageKeys.Sampled] = context.Sampled.ToString();

            foreach (var kvp in context.GetBaggageItems())
            {
                carrier.TextMap[BaggageKeys.BaggagePrefix + kvp.Key] = kvp.Value;
            }
        }

        public SpanContext MapCarrierToContext(TextMapCarrier carrier)
        {
            // we can't create a reference without a trace-id
            Guid? traceId = TryGetGuid(carrier, BaggageKeys.TraceId);
            if (!traceId.HasValue)
                return null;
            
            // something is seriously wrong if we have a trace-id but no span-id
            Guid? spanId = TryGetGuid(carrier, BaggageKeys.SpanId);
            if (!spanId.HasValue)
                return null;

            bool sampled = false;
            string sampledString;
            if (carrier.TextMap.TryGetValue(BaggageKeys.Sampled, out sampledString))
            {
                bool.TryParse(sampledString, out sampled);
            }
            
            var baggage = new Baggage();

            foreach (var kvp in carrier.TextMap)
            {
                if (kvp.Key.StartsWith(BaggageKeys.BaggagePrefix, StringComparison.OrdinalIgnoreCase))
                {
                    var key = kvp.Key.Substring(BaggageKeys.BaggagePrefix.Length);
                    baggage.Set(key, kvp.Value);
                }
            }

            return new SpanContext(traceId.Value, spanId.Value, sampled, baggage);
        }

        private Guid? TryGetGuid(TextMapCarrier carrier, string key)
        {
            string strValue;
            if (!carrier.TextMap.TryGetValue(key, out strValue))
                return null;

            Guid guidValue;
            if (!Guid.TryParse(strValue, out guidValue))
                return null;

            if (guidValue == Guid.Empty)
                return null;

            return guidValue;
        }
    }
}