// using System.Collections.Generic;
// using System.Linq;
// using OpenTracing.Propagation;
// using System.Collections.ObjectModel;

// namespace OpenTracing.BasicTracer.Propagation
// {
//     public class OpenTracingSpanContextToTextMapper : IContextTextMapMapper<OpenTracingSpanContext>
//     {
//         private const string prefixTracerState = "ot-tracer-";
//         private const string prefixBaggage = "ot-baggage-";

//         private const string fieldNameTraceID = prefixTracerState + "traceid";
//         private const string fieldNameSpanID = prefixTracerState + "spanid";
//         private const string fieldNameSampled = prefixTracerState + "sampled";

//         public IReadOnlyDictionary<string, string> MapFrom(OpenTracingSpanContext spanContext)
//         {
//             var baggageInfo = spanContext.GetBaggageItems().ToDictionary(p => prefixBaggage + p.Key, p => p.Value);

//             var tracerInfo = new Dictionary<string, string>()
//             {
//                 { fieldNameTraceID, spanContext.TraceId.ToString() },
//                 { fieldNameSpanID, spanContext.SpanId.ToString() },
//                 { fieldNameSampled, spanContext.Sampled.ToString() },
//             };

//             return new ReadOnlyDictionary<string, string>(baggageInfo.Union(tracerInfo).ToDictionary(p => p.Key, p => p.Value));
//         }

//         public bool TryMapTo(IReadOnlyDictionary<string, string> data, out OpenTracingSpanContext spanContext)
//         {
//             spanContext = null;

//             var lowercaseProperties = data.ToDictionary(p => p.Key.ToLower(), p => p.Value);

//             if (!lowercaseProperties.ContainsKey(fieldNameTraceID) ||
//                 !lowercaseProperties.ContainsKey(fieldNameSpanID))
//             {
//                 return false;
//             }

//             ulong traceId;
//             var traceIdParceResult = ulong.TryParse(lowercaseProperties[fieldNameTraceID], out traceId);

//             ulong parentId;
//             var parentIdParseResult = ulong.TryParse(lowercaseProperties[fieldNameSpanID], out parentId);

//             if (!traceIdParceResult || !parentIdParseResult)
//             {
//                 return false;
//             }

//             bool sampled;
//             if (lowercaseProperties.ContainsKey(fieldNameSampled))
//             {
//                 sampled = bool.TryParse(lowercaseProperties[fieldNameSampled], out sampled) ? sampled : false;
//             }
//             else
//             {
//                 sampled = false;
//             }

//             var baggage = lowercaseProperties
//                 .Where(p => p.Key.StartsWith(prefixBaggage))
//                 .ToDictionary(p => p.Key.Substring(prefixBaggage.Length, p.Key.Length), p => p.Value);

//             spanContext = new OpenTracingSpanContext(traceId, parentId, GuidFactory.Create(), sampled, baggage);

//             return true;
//         }
//     }
// }
