using OpenTracing.OpenTracing.Context;
using System;
using System.Collections.Generic;

namespace OpenTracing.OpenTracing.Tracer
{
    public class StartSpanOptions
    {
        public string OperationName { get; set; }
        public DateTime StartTime { get; set; } = DateTime.Now;
        public Dictionary<string, string> Tag { get; set; } = new Dictionary<string, string>() { };
    }
}
