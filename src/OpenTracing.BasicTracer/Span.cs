using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace OpenTracing.BasicTracer
{
    public static class SpanExtensions
    {
        public static SpanContext TypedContext(this ISpan span) => (SpanContext)span.Context;
    }

    public class Span : ISpan
    {
        private readonly ISpanRecorder _spanRecorder;

        public ISpanContext Context { get; }

        public ITracer Tracer { get; }
        public string OperationName { get; private set; }
        public DateTimeOffset StartTimestamp { get; }
        public DateTimeOffset? FinishTimestamp { get; private set; }

        public bool Finished { get; private set; }

        public IList<Tuple<string, ISpanContext>> References { get; } = new List<Tuple<string, ISpanContext>>();
        public IDictionary<string, object> Tags { get; } = new Dictionary<string, object>();
        public IList<LogData> Logs { get; } = new List<LogData>();

        internal Span(ITracer tracer, ISpanRecorder spanRecorder, ISpanContext context, string operationName, DateTimeOffset startTimestamp)
        {
            if (tracer == null)
            {
                throw new ArgumentNullException(nameof(tracer));
            }

            if (spanRecorder == null)
            {
                throw new ArgumentNullException(nameof(spanRecorder));
            }

            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            _spanRecorder = spanRecorder;

            Tracer = tracer;
            Context = context;
            StartTimestamp = startTimestamp;

            SetOperationName(operationName);
        }

        public virtual ISpan SetOperationName(string operationName)
        {
            if (string.IsNullOrWhiteSpace(operationName))
            {
                throw new ArgumentNullException(operationName);
            }

            OperationName = operationName.Trim();
            return this;
        }

        public virtual ISpan AddReference(string referenceType, ISpanContext spanContext)
        {
            if (string.IsNullOrWhiteSpace(referenceType))
            {
                throw new ArgumentNullException(nameof(referenceType));
            }

            if (spanContext == null)
            {
                throw new ArgumentNullException(nameof(spanContext));
            }

            References.Add(new Tuple<string, ISpanContext>(referenceType, spanContext));
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

        public virtual string GetBaggageItem(string key)
        {
            return Context.GetBaggageItem(key);
        }

        public virtual ISpan SetBaggageItem(string key, string value)
        {
            Context.SetBaggageItem(key, value);
            return this;
        }

        public virtual void Finish()
        {
            Finish(DateTimeOffset.UtcNow);
        }

        public virtual void Finish(DateTimeOffset finishTimestamp)
        {
            if (!Finished)
            {
                FinishTimestamp = finishTimestamp;
                Finished = true;
                OnFinished();
            }
        }

        protected void OnFinished()
        {
            var spanData = new SpanData()
            {
                Context = (SpanContext)Context,
                OperationName = OperationName,
                StartTimestamp = StartTimestamp,
                Duration = FinishTimestamp.Value - StartTimestamp,
                Tags = new ReadOnlyDictionary<string, object>(Tags),
                LogData = new ReadOnlyCollection<LogData>(Logs),
            };

            _spanRecorder.RecordSpan(spanData);
        }
    }
}