using OpenTracing.BasicTracer.Context;
using System;
using System.Collections.Generic;

namespace OpenTracing.BasicTracer
{
    public class Span : ISpan
    {
        private readonly ISpanRecorder _spanRecorder;

        private readonly SpanContext _context;

        public ISpanContext Context => _context;

        public string OperationName { get; private set; }
        public DateTimeOffset StartTimestamp { get; }
        public DateTimeOffset? FinishTimestamp { get; private set; }

        public IDictionary<string, object> Tags { get; } = new Dictionary<string, object>();
        public IList<LogData> Logs { get; } = new List<LogData>();

        internal Span(
            ISpanRecorder spanRecorder,
            SpanContext context,
            string operationName,
            DateTimeOffset startTimestamp,
            IDictionary<string, object> tags)
        {
            if (spanRecorder == null)
            {
                throw new ArgumentNullException(nameof(spanRecorder));
            }

            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (string.IsNullOrWhiteSpace(operationName))
            {
                throw new ArgumentNullException(operationName);
            }

            _spanRecorder = spanRecorder;

            _context = context;
            OperationName = operationName.Trim();
            StartTimestamp = startTimestamp;

            if (tags != null)
            {
                foreach (var tag in tags)
                {
                    Tags.Add(tag);
                }
            }
        }

        public virtual ISpan SetOperationName(string operationName)
        {
            if (string.IsNullOrWhiteSpace(operationName))
            {
                throw new ArgumentNullException(nameof(operationName));
            }

            OperationName = operationName;
            return this;
        }

        public virtual ISpan SetTag(string key, object value)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            Tags[key] = value;
            return this;
        }

        public virtual ISpan LogEvent(string eventName, object payload = null)
        {
            return LogEvent(DateTimeOffset.UtcNow, eventName, payload);
        }

        public virtual ISpan LogEvent(DateTimeOffset timestamp, string eventName, object payload = null)
        {
            if (string.IsNullOrWhiteSpace(eventName))
            {
                throw new ArgumentNullException(nameof(eventName));
            }

            Logs.Add(new LogData(timestamp, eventName, payload));
            return this;
        }

        public ISpan SetBaggageItem(string key, string value)
        {
            _context.SetBaggageItem(key, value);
            return this;
        }

        public string GetBaggageItem(string key)
        {
            return _context.GetBaggageItem(key);
        }

        public virtual void Finish(DateTimeOffset? finishTimestamp = null)
        {
            if (FinishTimestamp.HasValue)
                return;

            FinishTimestamp = finishTimestamp ?? DateTimeOffset.UtcNow;
            OnFinished();
        }

        protected void OnFinished()
        {
            var spanData = new SpanData()
            {
                Context = this.TypedContext(),
                OperationName = OperationName,
                StartTimestamp = StartTimestamp,
                Duration = FinishTimestamp.Value - StartTimestamp,
                Tags = Tags,
                LogData = Logs,
            };

            _spanRecorder.RecordSpan(spanData);
        }

        public void Dispose()
        {
            Finish();
        }
    }
}