using System;
using System.Collections.Generic;

namespace OpenTracing.BasicTracer
{
    public class SpanData
    {
        public SpanContext Context { get; internal set; }
        public string OperationName { get; internal set; }
        public DateTimeOffset StartTimestamp { get; internal set; }
        public TimeSpan Duration { get; internal set; }
        public IDictionary<string, object> Tags { get; internal set; }
        public IList<LogData> LogData { get; internal set; }
    }
}
