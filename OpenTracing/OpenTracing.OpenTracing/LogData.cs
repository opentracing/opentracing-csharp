using System;

namespace OpenTracing
{
    /// <summary>
    /// LogData is data associated with a Span. Every LogData instance should
    /// specify at least one of LogEvent and/or Payload.
    /// </summary>
    public class LogData
    {
        /// <param name="timestamp">The timestamp of the log record.</param>
        /// <param name="logEvent">A stable name of some notable moment in the lifetime of a Span</param>
        /// <param name="payload">Extra potentially structured log data</param>
        public LogData(DateTime timestamp, string logEvent, object payload)
        {
            Timestamp = timestamp;
            LogEvent = logEvent;
            Payload = payload;
        }

        /// <summary>
        /// The timestamp of the log record.
        /// </summary>
        public DateTime Timestamp { get; private set; }

        // Event (if non-empty) should be the stable name of some notable moment in
        // the lifetime of a Span. For instance, a Span representing a browser page
        // load might add an Event for each of the Performance.timing moments
        // here: https://developer.mozilla.org/en-US/docs/Web/API/PerformanceTiming
        //
        // While it is not a formal requirement, Event strings will be most useful
        // if they are *not* unique; rather, tracing systems should be able to use
        // them to understand how two similar Spans relate from an internal timing
        // perspective.
        public string LogEvent { get; private set; }

        // Payload is a free-form potentially structured object which Tracer
        // implementations may retain and record all, none, or part of.
        //
        // If included, `Payload` should be restricted to data derived from the
        // instrumented application; in particular, it should not be used to pass
        // semantic flags to a Log() implementation.
        //
        // For example, an RPC system could log the wire contents in both
        // directions, or a SQL library could log the query (with or without
        // parameter bindings); tracing implementations may truncate or otherwise
        // record only a snippet of these payloads (or may strip out PII, etc,
        // etc).
        public object Payload { get; private set; }
    }
}
