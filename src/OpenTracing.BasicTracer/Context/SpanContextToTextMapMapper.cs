using OpenTracing.Propagation;
using System.Collections.Generic;
using System.Linq;

namespace OpenTracing.BasicTracer.Context
{
    public class SpanContextToTextMapMapper : IContextMapper<TextMapFormat>
    {
        private const string prefixTracerState = "ot-tracer-";
        private const string prefixBaggage = "ot-baggage-";

        private const string fieldNameTraceID = prefixTracerState + "traceid";
        private const string fieldNameSpanID = prefixTracerState + "spanid";
        private const string fieldNameParentID = prefixTracerState + "parentid";
        private const string fieldNameSampled = prefixTracerState + "sampled";

        public TextMapFormat MapFrom(SpanContext spanContext)
        {
            var baggageInfo = spanContext.GetBaggageItems().ToDictionary(p => prefixBaggage + p.Key, p => p.Value);

            var tracerInfo = new Dictionary<string, string>()
            {
                { fieldNameTraceID, spanContext.TraceId.ToString() },
                { fieldNameSpanID, spanContext.SpanId.ToString() },
                { fieldNameParentID, spanContext.ParentId.ToString() },
                { fieldNameSampled, spanContext.Sampled.ToString() },
            };

            return new TextMapFormat(baggageInfo.Union(tracerInfo).ToDictionary(p => p.Key, p => p.Value));
        }

        public SpanContext MapTo(TextMapFormat data)
        {
            var lowercaseProperties = data.ToDictionary(p => p.Key.ToLower(), p => p.Value);

            if (!lowercaseProperties.ContainsKey(fieldNameTraceID) ||
                !lowercaseProperties.ContainsKey(fieldNameSpanID))
            {
                return null;
            }

            ulong traceId;
            var traceIdParceResult = ulong.TryParse(lowercaseProperties[fieldNameTraceID], out traceId);

            ulong parentId;
            var parentIdParseResult = ulong.TryParse(lowercaseProperties[fieldNameParentID], out parentId);

            ulong spanId;
            var spanIdParseResult = ulong.TryParse(lowercaseProperties[fieldNameSpanID], out spanId);

            if (!traceIdParceResult || !parentIdParseResult)
            {
                return null;
            }

            bool sampled;
            if (lowercaseProperties.ContainsKey(fieldNameSampled))
            {
                sampled = bool.TryParse(lowercaseProperties[fieldNameSampled], out sampled) ? sampled : false;
            }
            else
            {
                sampled = false;
            }

            var keyValuePairList = lowercaseProperties
                .Where(p => p.Key.StartsWith(prefixBaggage))
                .Select(p => new KeyValuePair<string, string>(p.Key.Substring(prefixBaggage.Length, p.Key.Length), p.Value)).ToList();
            var baggage = new Baggage();

            baggage.Merge(keyValuePairList);

            return new SpanContext(traceId, parentId, spanId, sampled, baggage);
        }
    }
}
