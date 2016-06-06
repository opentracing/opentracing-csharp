using System;

namespace OpenTracing
{
    public class LogData
    {
        public LogData(DateTime dateTime, string message, object obj)
        {
            DateTime = dateTime;
            Message = message;
            Obj = obj;
        }

        public DateTime DateTime { get; private set; }
        public string Message { get; private set; }
        public object Obj { get; private set; }
    }
}
