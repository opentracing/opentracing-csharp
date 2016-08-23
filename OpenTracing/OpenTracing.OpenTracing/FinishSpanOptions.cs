using System;
using System.Collections.Generic;

namespace OpenTracing
{
    /// <summary>
    /// FinishOptions allows Span.FinishWithOptions callers to override the finish
    /// timestamp and provide log data via a bulk interface.
    /// </summary>
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

        /// <summary>
        /// FinishTime overrides the Span's finish time.
        ///
        /// FinishTime must resolve to a timestamp that's >= the Span's StartTime
        /// (per StartSpanOptions).
        /// </summary>
        public DateTime FinishTime { get; private set; }

        /// <summary>
        /// LogData allows the caller to specify the contents of many Log()
        /// calls with a single slice. May be nil.
        ///
        /// None of the LogData.Timestamp values may be null (i.e., they must
        /// be set explicitly). Also, they must be &gt;= the Span's start timestamp
        /// and &lt;= the FinishTime (or time.Now() if FinishTime == null). Otherwise 
        /// the behavior of FinishWithOptions() is undefined.
        ///
        /// If specified, the caller hands off ownership of LogData at
        /// FinishWithOptions() invocation time.
        /// </summary>
        public List<LogData> LogData { get; private set; }
    }
}