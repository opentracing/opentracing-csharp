using System;

namespace OpenTracing.BasicTracer
{
    public struct LogData
    {
        public LogData(DateTimeOffset timestamp, string eventName, object payload)
        {
            Timestamp = timestamp;
            EventName = eventName;
            Payload = payload;
        }

        public DateTimeOffset Timestamp { get; private set; }
        public string EventName { get; private set; }
        public object Payload { get; private set; }
    }
}
