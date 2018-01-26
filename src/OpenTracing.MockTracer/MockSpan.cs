using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace OpenTracing.MockTracer
{
    /// <inheritdoc cref="ISpan"/>
    public class MockSpan : ISpan
    {
        /// <summary>
        /// Used to monotonically update ids 
        /// </summary>
        private static long _nextIdCounter = 0;

        public static long NextId()
        {
            return Interlocked.Increment(ref _nextIdCounter);
        }

        public MockSpan(MockTracer tracer, string operationName, DateTimeOffset startTime, Dictionary<string, object> initialTags, List<Reference> refs)
        {
            StartTime = startTime;
            _tracer = tracer;
            OperationName = operationName;

            if (initialTags == null)
            {
                _tags = new Dictionary<string, object>();
            }
            else
            {
                _tags = initialTags;
            }

            if (_references == null)
            {
                _references = refs;
            }
            else
            {
                _references = new List<Reference>();
            }

            var parentContext = FindPreferredParentRef(_references);

            if (parentContext == null) // we are a root span
            {
                _context = new MockContext(NextId(), NextId(), new Dictionary<string, string>());
                ParentId = 0;
            }
            else // we are a child span
            {
                _context = new MockContext(parentContext.TraceId, NextId(), MergeBaggages(_references));
                ParentId = parentContext.SpanId;
            }
           
        }

        private MockContext _context;

        /// <summary>
        /// Id of the parent span. Set to 0 if there is no parent.
        /// </summary>
        public long ParentId { get; }

        public long TraceId => _context.TraceId;
        public long SpanId => _context.SpanId;

        private readonly MockTracer _tracer;

        public DateTimeOffset StartTime { get; private set; }
        public DateTimeOffset FinishTime { get; private set; }

        public string OperationName { get; private set; }
        private readonly Dictionary<string, object> _tags;
        private readonly List<LogEvent> _logs = new List<LogEvent>();
        private readonly List<Reference> _references;
        private readonly ConcurrentBag<Exception> _exceptions = new ConcurrentBag<Exception>();

        public IReadOnlyCollection<Exception> Errors => _exceptions.ToList(); // don't need this ToList call in later versions of .NET Standard

        public bool IsFinished { get; private set; }

        public IReadOnlyList<LogEvent> Logs => _logs;

        public IReadOnlyDictionary<string, object> Tags => _tags;

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool isDisposing)
        {
            if (isDisposing && !IsFinished)
            {
                Finish();
            }
        }

        public ISpan SetOperationName(string operationName)
        {
            CheckForFinished("Setting operationName [{0}] on already finished span", operationName);
            OperationName = operationName;
            return this;
        }

        public ISpan SetTag(string key, bool value)
        {
            return SetObjectTag(key, value);
        }

        public ISpan SetTag(string key, double value)
        {
            return SetObjectTag(key, value);
        }

        public ISpan SetTag(string key, int value)
        {
            return SetObjectTag(key, value);
        }

        public ISpan SetTag(string key, string value)
        {
            return SetObjectTag(key, value);
        }

        private ISpan SetObjectTag(string key, object value)
        {
            CheckForFinished("Setting tag [{0}:{1}] on already finished span", key, value);
            _tags[key] = value;
            return this;
        }

        public ISpan Log(IEnumerable<KeyValuePair<string, object>> fields)
        {
            return Log(new LogEvent(fields.ToDictionary(k => k.Key, v => v.Value)));
        }
        public ISpan Log(DateTimeOffset timestamp, IEnumerable<KeyValuePair<string, object>> fields)
        {
            return Log(new LogEvent(timestamp, fields.ToDictionary(k => k.Key, v => v.Value)));
        }

        public ISpan Log(string eventName)
        {
            return Log(new LogEvent(eventName));
        }

        public ISpan Log(DateTimeOffset timestamp, string eventName)
        {
            return Log(new LogEvent(timestamp, eventName));
        }

        private ISpan Log(LogEvent log)
        {
            CheckForFinished("Adding logs {0} to already finished span.", log);
            _logs.Add(log);
            return this;
        }

        public ISpan SetBaggageItem(string key, string value)
        {
            CheckForFinished("Adding baggage [{0}:{1}] to already finished span.", key, value);
            _context = _context.WithBaggageItem(key, value);
            return this;
        }

        public string GetBaggageItem(string key)
        {
            return _context.GetBaggageItem(key);
        }

        public void Finish()
        {
            Finish(DateTimeOffset.UtcNow);
        }

        public void Finish(DateTimeOffset finishTimestamp)
        {
            CheckForFinished("Tried to finish already finished span");
            FinishTime = finishTimestamp;
            _tracer.FinishSpan(this);
            IsFinished = true;
        }

        public ISpan AddReference(Reference reference)
        {
            _references.Add(reference);
            return this;
        }

        public IReadOnlyList<Reference> References => _references;

        public ISpanContext Context => _context;

        private static MockContext FindPreferredParentRef(IList<Reference> references)
        {
            if (!references.Any())
                return null;

            foreach (var reference in references) // return the context of the parent, if applicable
            {
                if (OpenTracing.References.ChildOf.Equals(reference.ReferenceType))
                    return reference.Context;
            }

            // otherwise, return the context of the first reference
            return references.First().Context;
        }

        private static Dictionary<string, string> MergeBaggages(IList<Reference> references)
        {
            var baggage = new Dictionary<string, string>();
            foreach (var reference in references)
            {
                if (reference.Context.GetBaggageItems() != null)
                {
                    foreach (var bagItem in reference.Context.GetBaggageItems())
                    {
                        baggage[bagItem.Key] = bagItem.Value;
                    }
                }
            }

            return baggage;
        }

        private void CheckForFinished(string format, params object[] args)
        {
            if (IsFinished)
            {
                var ex = new InvalidOperationException(string.Format(format, args));
                _exceptions.Add(ex);
                throw ex;
            }
        }

        public override string ToString()
        {
            return $"TraceId: {TraceId}, SpanId: {SpanId}, ParentId: {ParentId}, OperationName: {OperationName}";
        }

        /// <summary>
        /// INTERNAL API.
        /// 
        /// Internal representation of a log entry inside <see cref="ISpan"/>
        /// </summary>
        public sealed class LogEvent
        {
            public LogEvent(string eventName)
                : this((IReadOnlyDictionary<string, object>)new Dictionary<string, object>() { { "event", eventName } }) { }

            public LogEvent(DateTimeOffset timestamp, string eventName)
                : this(timestamp, (IReadOnlyDictionary<string, object>)new Dictionary<string, object>() { { "event", eventName } }) { }

            public LogEvent(IReadOnlyDictionary<string, object> fields)
                : this(DateTimeOffset.UtcNow, fields)
            {
            }

            public LogEvent(DateTimeOffset timeStamp, IReadOnlyDictionary<string, object> fields)
            {
                TimeStamp = timeStamp;
                Fields = fields;
            }

            public DateTimeOffset TimeStamp { get; private set; }

            public IReadOnlyDictionary<string, object> Fields { get; private set; }
        }

        public sealed class MockContext : ISpanContext
        {
            private readonly IDictionary<string, string> _baggageItems;

            public long TraceId { get; private set; }

            public long SpanId { get; private set; }

            public MockContext(long traceId, long spanId) : this(traceId, spanId, new Dictionary<string, string>()) { }

            public MockContext(long traceId, long spanId, IDictionary<string, string> baggageItems)
            {
                TraceId = traceId;
                SpanId = spanId;
                _baggageItems = baggageItems;
            }

            public IEnumerable<KeyValuePair<string, string>> GetBaggageItems()
            {
                return _baggageItems;
            }

            public string GetBaggageItem(string key)
            {
                if(_baggageItems.ContainsKey(key))
                    return _baggageItems[key];
                return null;
            }

            public MockContext WithBaggageItem(string key, string val)
            {
                return new MockContext(TraceId, SpanId, _baggageItems.Concat(new[] { new KeyValuePair<string, string>(key, val) })
                    .ToDictionary(k => k.Key, v => v.Value));
            }
        }

        public class Reference
        {
            public Reference(MockContext context, string referenceType)
            {
                Context = context;
                ReferenceType = referenceType;
            }

            public MockContext Context { get; }

            /// <summary>
            /// Per <see cref="References"/>
            /// </summary>
            public string ReferenceType { get; }

            protected bool Equals(Reference other)
            {
                return Context.Equals(other.Context);
            }

            public override bool Equals(object obj)
            {
                if (Object.ReferenceEquals(null, obj)) return false;
                if (Object.ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((Reference)obj);
            }

            public override int GetHashCode()
            {
                return Context.GetHashCode();
            }
        }
    }
}