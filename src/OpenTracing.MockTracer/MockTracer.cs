using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using OpenTracing.Propagation;

namespace OpenTracing.MockTracer
{
    /// <summary>
    /// In-memory <see cref="ITracer"/> implementation designed to make it
    /// easy to test assertions against the OpenMetrics API and its use
    /// inside frameworks.
    /// </summary>
    public class MockTracer : ITracer
    {
        private ConcurrentQueue<MockSpan> _finishedSpans = new ConcurrentQueue<MockSpan>();
        private readonly IPropagator _propagator;

        public static readonly TextPropagator TextMapPropagator = new TextPropagator();

        public MockTracer() : this(TextMapPropagator)
        {
        }

        public MockTracer(IPropagator propagator)
        {
            _propagator = propagator;
        }

        public void FinishSpan(MockSpan span)
        {
            _finishedSpans.Enqueue(span);
        }

        public IEnumerable<MockSpan> FinishedSpans => _finishedSpans;

        /// <summary>
        /// Clears all of the current recorded <see cref="ISpan"/> instances.
        /// </summary>
        public void Reset()
        {
            _finishedSpans = new ConcurrentQueue<MockSpan>();
        }

        public ISpanBuilder BuildSpan(string operationName)
        {
            return new MockSpanBuilder(this, operationName);
        }

        public void Inject<TCarrier>(ISpanContext spanContext, Format<TCarrier> format, TCarrier carrier)
        {
            _propagator.Inject((MockSpan.MockContext)spanContext, format, carrier);
        }

        public ISpanContext Extract<TCarrier>(Format<TCarrier> format, TCarrier carrier)
        {
            return _propagator.Extract(format, carrier);
        }

        /// <summary>
        /// Allows the developer to inject into the <see cref="MockTracer.Inject"/> and <see cref="MockTracer.Extract"/> calls.
        /// </summary>
        public interface IPropagator
        {
            void Inject<TCarrier>(MockSpan.MockContext context, Format<TCarrier> format, TCarrier carrier);

            MockSpan.MockContext Extract<TCarrier>(Format<TCarrier> format, TCarrier carrier);
        }

        /// <summary>
        /// <see cref="IPropagator"/> implementation that uses <see cref="ITextMap"/> internally.
        /// </summary>
        public sealed class TextPropagator : IPropagator
        {
            public const string SpanIdKey = "spanid";
            public const string TraceIdKey = "traceid";
            public const string BaggageKeyPrefix = "baggage-";

            public void Inject<TCarrier>(MockSpan.MockContext context, Format<TCarrier> format, TCarrier carrier)
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
                    throw new UnsupportedFormatException($"unknown carrier [{carrier.GetType()}]");
                }
            }

            public MockSpan.MockContext Extract<TCarrier>(Format<TCarrier> format, TCarrier carrier)
            {
                long? traceId = null;
                long? spanId = null;
                Dictionary<string, string> baggage = new Dictionary<string, string>();

                if (carrier is ITextMap text)
                {
                    foreach (var entry in text.GetEntries())
                    {
                        if (TraceIdKey.Equals(entry.Key))
                        {
                            traceId = Convert.ToInt64(entry.Value);
                        }
                        else if (SpanIdKey.Equals(entry.Key))
                        {
                            spanId = Convert.ToInt64(entry.Value);
                        }
                        else if(entry.Key.StartsWith(BaggageKeyPrefix))
                        {
                            var key = entry.Key.Substring(BaggageKeyPrefix.Length);
                            baggage[key] = entry.Value;
                        }
                    }
                }
                else
                {
                    throw new UnsupportedFormatException($"unknown carrier [{carrier.GetType()}]");
                }

                if (traceId.HasValue && spanId.HasValue)
                {
                    return new MockSpan.MockContext(traceId.Value, spanId.Value, baggage);
                }

                return null;
            }
        }

        public sealed class MockSpanBuilder : ISpanBuilder
        {
            private readonly MockTracer _tracer;
            private readonly string _operationName;
            private DateTimeOffset _startTime = DateTimeOffset.MinValue;
            private readonly List<MockSpan.Reference> _spanReferences = new List<MockSpan.Reference>();
            private readonly Dictionary<string, object> _initialTags = new Dictionary<string, object>();

            public MockSpanBuilder(MockTracer tracer, string operationName)
            {
                _tracer = tracer;
                _operationName = operationName;
            }

            public ISpanBuilder AsChildOf(ISpan parent)
            {
                if (parent == null) 
                    return this;
                return AsChildOf(parent.Context);
            }

            public ISpanBuilder AsChildOf(ISpanContext parent)
            {
                if (parent == null)
                    return this;
                return AddReference(References.ChildOf, parent);
            }

            public ISpanBuilder FollowsFrom(ISpan parent)
            {
                return FollowsFrom(parent.Context);
            }

            public ISpanBuilder FollowsFrom(ISpanContext parent)
            {
                return AddReference(References.FollowsFrom, parent);
            }

            public ISpanBuilder AddReference(string referenceType, ISpanContext referencedContext)
            {
                if (referencedContext != null)
                {
                    _spanReferences.Add(new MockSpan.Reference((MockSpan.MockContext)referencedContext, referenceType));
                }

                return this;
            }

            public ISpanBuilder WithTag(string key, bool value)
            {
                _initialTags[key] = value;
                return this;
            }

            public ISpanBuilder WithTag(string key, double value)
            {
                _initialTags[key] = value;
                return this;
            }

            public ISpanBuilder WithTag(string key, int value)
            {
                _initialTags[key] = value;
                return this;
            }

            public ISpanBuilder WithTag(string key, string value)
            {
                _initialTags[key] = value;
                return this;
            }

            public ISpanBuilder WithStartTimestamp(DateTimeOffset startTimestamp)
            {
                _startTime = startTimestamp;
                return this;
            }

            public ISpan Start()
            {
                if (_startTime == DateTimeOffset.MinValue) // value was not set by builder
                {
                    _startTime = DateTimeOffset.UtcNow;
                }

                return new MockSpan(_tracer, _operationName, _startTime, _initialTags, _spanReferences);
            }
        }
    }


}
