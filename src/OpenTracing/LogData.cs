using System;

namespace OpenTracing
{
    public struct LogData
    {
        // TODO should this be in this library?
        public LogData(DateTimeOffset timestamp, string eventName, object payload)
        {
            Timestamp = timestamp.ToUniversalTime();
            EventName = eventName;
            Payload = payload;
        }

        public DateTimeOffset Timestamp { get; private set; }
        public string EventName { get; private set; }
        public object Payload { get; private set; }
    }
}
