using System;
using System.Collections.Generic;
using OpenTracing.Propagation;

namespace OpenTracing.Mock
{
    public static class Propagators
    {
        public static readonly IPropagator Console = new ConsolePropagator();

        public static readonly IPropagator TextMap = new TextMapPropagator();
    }

    /// <summary>
    /// Allows the developer to inject into the <see cref="MockTracer.Inject"/> and <see cref="MockTracer.Extract"/> calls.
    /// </summary>
    public interface IPropagator
    {
        void Inject<TCarrier>(MockSpanContext context, IFormat<TCarrier> format, TCarrier carrier);

        MockSpanContext Extract<TCarrier>(IFormat<TCarrier> format, TCarrier carrier);
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
            long? traceId = null;
            long? spanId = null;
            Dictionary<string, string> baggage = new Dictionary<string, string>();

            if (carrier is ITextMap text)
            {
                foreach (var entry in text)
                {
                    if (TraceIdKey.Equals(entry.Key))
                    {
                        traceId = Convert.ToInt64(entry.Value);
                    }
                    else if (SpanIdKey.Equals(entry.Key))
                    {
                        spanId = Convert.ToInt64(entry.Value);
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

            if (traceId.HasValue && spanId.HasValue)
            {
                return new MockSpanContext(traceId.Value, spanId.Value, baggage);
            }

            return null;
        }
    }
}
