using OpenTracing.OpenTracing.Context;
using System;
using System.Collections.Generic;

namespace OpenTracing.OpenTracing.Tracer
{
    public class StartSpanOptions<T> where T : ISpanContext
    {
        public string OperationName { get; set; }
        public T ParentContext { get; set; }
        public DateTime StartTime { get; set; } = DateTime.Now;
        public Dictionary<string, string> Tag { get; set; } = new Dictionary<string, string>() { };

        public ISpanRecorder<T> SpanRecorder { get; set; }
    }
}
