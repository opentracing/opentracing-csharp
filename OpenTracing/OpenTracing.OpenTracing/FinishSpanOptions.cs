using System;
using System.Collections.Generic;

namespace OpenTracing
{
    public class FinishSpanOptions
    {
        public FinishSpanOptions(DateTime finishTime)
            : this(finishTime, null)
        {
        }

        public FinishSpanOptions(DateTime finishTime, List<LogData> logData)
        {
            FinishTime = finishTime;
            LogData = logData;
        }

        public DateTime FinishTime { get; private set; }

        public List<LogData> LogData { get; private set; }
    }
}