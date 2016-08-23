﻿using System;
using System.Collections.Generic;

namespace OpenTracing.BasicTracer
{
    public class SpanData<TContext>
    {
        public TContext Context { get; internal set; }
         
        public string OperationName { get; internal set; }
        public DateTime StartTime { get; internal set; }
        public TimeSpan Duration { get; internal set; }
        public IReadOnlyDictionary<string, string> Tags { get; internal set; }
        public IReadOnlyList<LogData> LogData { get; internal set; }
        public List<SpanReference> References { get; internal set; }
    }
}
