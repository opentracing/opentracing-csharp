using System;
using System.Collections.Generic;
using System.IO;
using OpenTracing.Propagation;

namespace OpenTracing.Mock
{
    public static class Propagators
    {
        public static readonly IPropagator Console = new ConsolePropagator();

        public static readonly IPropagator TextMap = new TextMapPropagator();

        public static readonly IPropagator Binary = new BinaryPropagator();
    }

    /// <summary>
    /// Allows the developer to inject into the <see cref="MockTracer.Inject{TCarrier}"/> and <see cref="MockTracer.Extract{TCarrier}"/> calls.
    /// </summary>
    public interface IPropagator
    {
        void Inject<TCarrier>(MockSpanContext context, IFormat<TCarrier> format, TCarrier carrier);

        MockSpanContext Extract<TCarrier>(IFormat<TCarrier> format, TCarrier carrier);
    }

    public sealed class BinaryPropagator : IPropagator
    {
        public class BinaryContext
        {
            public string TraceId { get; set; }
            public string SpanId { get; set; }
        }
        
        public void Inject<TCarrier>(MockSpanContext context, IFormat<TCarrier> format, TCarrier carrier)
        {
            if (carrier is IBinary stream)
            {
                var contextObject = new BinaryContext
                {
                    SpanId = context.SpanId, TraceId = context.TraceId
                };
                var serialContext = Serialize(contextObject);
                stream.Set(serialContext);
            }
            else
            {
                throw new InvalidOperationException($"Unknown carrier [{carrier.GetType()}]");
            }
        }

        public MockSpanContext Extract<TCarrier>(IFormat<TCarrier> format, TCarrier carrier)
        {
            string traceId = "";
            string spanId = "";

            if (carrier is IBinary stream)
            {
                var ctx = Deserialize(stream.Get());
                traceId = ctx.TraceId;
                spanId = ctx.SpanId;
            }
            else
            {
                throw new InvalidOperationException($"Unknown carrier [{carrier.GetType()}]");
            }
            
            if (!string.IsNullOrEmpty(traceId) && !string.IsNullOrEmpty(spanId))
            {
                return new MockSpanContext(traceId, spanId, null);
            }

            return null;
        }

        public MemoryStream Serialize(BinaryContext ctx)
        {
            var ms = new MemoryStream();
            using (var writer = new BinaryWriter(ms, System.Text.Encoding.UTF8, true))
            {
                writer.Write(ctx.SpanId);
                writer.Write(ctx.TraceId);
            }
            return ms;
        }

        public BinaryContext Deserialize(MemoryStream stream)
        {
            stream.Position = 0;
            var res = new BinaryContext();
            using (var reader = new BinaryReader(stream))
            {
                res.SpanId = reader.ReadString();
                res.TraceId = reader.ReadString();
            }

            return res;
        }
    }

    public sealed class ConsolePropagator : IPropagator
    {
        public void Inject<TCarrier>(MockSpanContext context, IFormat<TCarrier> format, TCarrier carrier)
        {
            Console.WriteLine($"Inject({context}, {format}, {carrier}");
        }

        public MockSpanContext Extract<TCarrier>(IFormat<TCarrier> format, TCarrier carrier)
        {
            Console.WriteLine($"Extract({format}, {carrier}");
            return null;
        }
    }

    /// <summary>
    /// <see cref="IPropagator"/> implementation that uses <see cref="ITextMap"/> internally.
    /// </summary>
    public sealed class TextMapPropagator : IPropagator
    {
        public const string SpanIdKey = "spanid";
        public const string TraceIdKey = "traceid";
        public const string BaggageKeyPrefix = "baggage-";

        public void Inject<TCarrier>(MockSpanContext context, IFormat<TCarrier> format, TCarrier carrier)
        {
            if (carrier is ITextMap text)
            {
                foreach (var entry in context.GetBaggageItems())
                {
                    text.Set(BaggageKeyPrefix + entry.Key, entry.Value);
                }

                text.Set(SpanIdKey, context.SpanId.ToString());
                text.Set(TraceIdKey, context.TraceId.ToString());
            }
            else
            {
                throw new InvalidOperationException($"Unknown carrier [{carrier.GetType()}]");
            }
        }

        public MockSpanContext Extract<TCarrier>(IFormat<TCarrier> format, TCarrier carrier)
        {
            string traceId = null;
            string spanId = null;
            Dictionary<string, string> baggage = new Dictionary<string, string>();

            if (carrier is ITextMap text)
            {
                foreach (var entry in text)
                {
                    if (TraceIdKey.Equals(entry.Key))
                    {
                        traceId = entry.Value;
                    }
                    else if (SpanIdKey.Equals(entry.Key))
                    {
                        spanId = entry.Value;
                    }
                    else if (entry.Key.StartsWith(BaggageKeyPrefix))
                    {
                        var key = entry.Key.Substring(BaggageKeyPrefix.Length);
                        baggage[key] = entry.Value;
                    }
                }
            }
            else
            {
                throw new InvalidOperationException($"Unknown carrier [{carrier.GetType()}]");
            }

            if (!string.IsNullOrEmpty(traceId) && !string.IsNullOrEmpty(spanId))
            {
                return new MockSpanContext(traceId, spanId, baggage);
            }

            return null;
        }
    }
}
