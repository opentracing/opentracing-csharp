using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;

namespace OpenTracing.Mock
{
    /// <summary>
    /// MockSpans are created via <see cref="MockTracer.BuildSpan"/>, but they are also returned via calls to
    /// <see cref="MockTracer.FinishedSpans"/>. They provide accessors to all span state.
    /// </summary>
    /// <seealso cref="MockTracer.FinishedSpans"/>
    public sealed class MockSpan : ISpan
    {
        /// <summary>
        /// Used to monotonically update ids
        /// </summary>
        private static long _nextIdCounter = 0;

        /// <summary>
        /// A simple-as-possible (consecutive for repeatability) id generator.
        /// </summary>
        private static long NextId()
        {
            return Interlocked.Increment(ref _nextIdCounter);
        }

        private readonly object _lock = new object();

        private readonly MockTracer _mockTracer;
        private MockSpanContext _context;
        private DateTimeOffset _finishTimestamp;
        private bool _finished;
        private readonly Dictionary<string, object> _tags;
        private readonly List<Reference> _references;
        private readonly List<LogEntry> _logEntries = new List<LogEntry>();
        private readonly List<Exception> _errors = new List<Exception>();

        /// <summary>
        /// The spanId of the span's first <see cref="References.ChildOf"/> reference, or the first reference of any type,
        /// or 0 if no reference exists.
        /// </summary>
        /// <seealso cref="MockSpanContext.SpanId"/>
        /// <seealso cref="MockSpan.References"/>
        public long ParentId { get; }

        public DateTimeOffset StartTimestamp { get; }

        /// <summary>
        /// The finish time of the span; only valid after a call to <see cref="Finish()"/>.
        /// </summary>
        public DateTimeOffset FinishTimestamp
        {
            get
            {
                if (_finishTimestamp == DateTimeOffset.MinValue)
                    throw new InvalidOperationException("Must call Finish() before FinishTimestamp");

                return _finishTimestamp;
            }
        }

        public string OperationName { get; private set; }

        /// <summary>
        /// A copy of all tags set on this span.
        /// </summary>
        public Dictionary<string, object> Tags => new Dictionary<string, object>(_tags);

        /// <summary>
        /// A copy of all log entries added to this span.
        /// </summary>
        public List<LogEntry> LogEntries => new List<LogEntry>(_logEntries);

        /// <summary>
        /// A copy of exceptions thrown by this class (e.g. adding a tag after span is finished).
        /// </summary>
        public List<Exception> GeneratedErrors => new List<Exception>(_errors);

        public List<Reference> References => new List<Reference>(_references);

        public MockSpanContext Context
        {
            // C# doesn't have "return type covariance" so we use the trick with the explicit interface implementation
            // and this separate property.
            get
            {
                lock (_lock)
                {
                    return _context;
                }
            }
        }

        ISpanContext ISpan.Context => Context;

        public MockSpan(
            MockTracer tracer,
            string operationName,
            DateTimeOffset startTimestamp,
            Dictionary<string, object> initialTags,
            List<Reference> references)
        {
            _mockTracer = tracer;
            OperationName = operationName;
            StartTimestamp = startTimestamp;

            _tags = initialTags == null
                ? new Dictionary<string, object>()
                : new Dictionary<string, object>(initialTags);

            _references = references == null
                ? new List<Reference>()
                : references.ToList();

            var parentContext = FindPreferredParentRef(_references);

            if (parentContext == null)
            {
                // we are a root span
                _context = new MockSpanContext(NextId(), NextId(), new Dictionary<string, string>());
                ParentId = 0;
            }
            else
            {
                // we are a child span
                _context = new MockSpanContext(parentContext.TraceId, NextId(), MergeBaggages(_references));
                ParentId = parentContext.SpanId;
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
            lock (_lock)
            {
                CheckForFinished("Setting tag [{0}:{1}] on already finished span", key, value);
                _tags[key] = value;
                return this;
            }
        }

        public ISpan Log(IDictionary<string, object> fields)
        {
            return Log(DateTimeOffset.UtcNow, fields);
        }

        public ISpan Log(DateTimeOffset timestamp, IDictionary<string, object> fields)
        {
            lock (_lock)
            {
                CheckForFinished("Adding logs {0} at {1} to already finished span.", fields, timestamp);
                _logEntries.Add(new LogEntry(timestamp, fields));
                return this;
            }
        }

        public ISpan Log(string @event)
        {
            return Log(DateTimeOffset.UtcNow, @event);
        }

        public ISpan Log(DateTimeOffset timestamp, string @event)
        {
            return Log(timestamp, new Dictionary<string, object> { { "event", @event } });
        }

        public ISpan SetBaggageItem(string key, string value)
        {
            lock (_lock)
            {
                CheckForFinished("Adding baggage [{0}:{1}] to already finished span.", key, value);
                _context = _context.WithBaggageItem(key, value);
                return this;
            }
        }

        public string GetBaggageItem(string key)
        {
            lock (_lock)
            {
                return _context.GetBaggageItem(key);
            }
        }

        public void Finish()
        {
            Finish(DateTimeOffset.UtcNow);
        }

        public void Finish(DateTimeOffset finishTimestamp)
        {
            lock (_lock)
            {
                CheckForFinished("Tried to finish already finished span");
                _finishTimestamp = finishTimestamp;
                _mockTracer.AppendFinishedSpan(this);
                _finished = true;
            }
        }

        private static MockSpanContext FindPreferredParentRef(IList<Reference> references)
        {
            if (!references.Any())
                return null;

            // return the context of the parent, if applicable
            foreach (var reference in references)
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
            if (_finished)
            {
                var ex = new InvalidOperationException(string.Format(format, args));
                _errors.Add(ex);
                throw ex;
            }
        }

        public override string ToString()
        {
            return $"TraceId: {_context.TraceId}, SpanId: {_context.SpanId}, ParentId: {ParentId}, OperationName: {OperationName}";
        }

        public sealed class LogEntry
        {
            public DateTimeOffset Timestamp { get; }

            public IReadOnlyDictionary<string, object> Fields { get; }

            public LogEntry(DateTimeOffset timestamp, IDictionary<string, object> fields)
            {
                Timestamp = timestamp;
                Fields = new ReadOnlyDictionary<string, object>(fields);
            }
        }

        public sealed class Reference : IEquatable<Reference>
        {
            public MockSpanContext Context { get; }

            /// <summary>
            /// See <see cref="OpenTracing.References"/>.
            /// </summary>
            public string ReferenceType { get; }

            public Reference(MockSpanContext context, string referenceType)
            {
                Context = context ?? throw new ArgumentNullException(nameof(context));
                ReferenceType = referenceType ?? throw new ArgumentNullException(nameof(referenceType));
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as Reference);
            }

            public bool Equals(Reference other)
            {
                return other != null &&
                       EqualityComparer<MockSpanContext>.Default.Equals(Context, other.Context) &&
                       ReferenceType == other.ReferenceType;
            }

            public override int GetHashCode()
            {
                var hashCode = 2083322454;
                hashCode = hashCode * -1521134295 + EqualityComparer<MockSpanContext>.Default.GetHashCode(Context);
                hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ReferenceType);
                return hashCode;
            }
        }
    }
}
