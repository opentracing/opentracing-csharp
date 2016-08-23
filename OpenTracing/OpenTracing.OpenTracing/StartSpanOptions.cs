using System;
using System.Collections.Generic;

namespace OpenTracing
{
    /// <summary>
    /// StartSpanOptions creates a mechanism to override the start timestamp, 
    /// specify Span References, and make a single Tag or multiple Tags available 
    /// at Span start time.
    /// </summary>
    public class StartSpanOptions
    {
        /// <summary>
        /// StartTime when the span began. DateTime.Now is default value.
        /// </summary>
        public DateTime StartTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, string> Tag { get; set; } = new Dictionary<string, string>() { };

        /// <summary>
        /// List of span references.
        /// </summary>
        public List<SpanReference> References = new List<SpanReference> { };
    }
}
