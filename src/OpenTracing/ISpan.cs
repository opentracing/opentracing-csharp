using System;

namespace OpenTracing
{
    public interface ISpan
    {
        ISpanContext Context { get; }

        ITracer Tracer { get; }

        ISpan SetOperationName(string operationName);

        ISpan AddReference(string referenceType, ISpanContext spanContext);

        ISpan SetTag(string key, object value);

        ISpan LogEvent(string eventName, object payload = null);
        ISpan LogEvent(DateTimeOffset timestamp, string eventName, object payload = null);

        string GetBaggageItem(string key);
        ISpan SetBaggageItem(string key, string value);

        void Finish();
        void Finish(DateTimeOffset finishTimestamp);
    }
}