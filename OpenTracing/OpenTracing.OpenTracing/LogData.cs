using System;

namespace OpenTracing
{
    public class LogData
    {
        public LogData(DateTime timestamp, string logEvent, object payload)
        {
            Timestamp = timestamp;
            LogEvent = logEvent;
            Payload = payload;
        }

        public DateTime Timestamp { get; private set; }
        public string LogEvent { get; private set; }
        public object Payload { get; private set; }
    }
}
