using System;

namespace OpenTracing
{
    /// <summary>
    /// Span represents an active, un-finished span in the OpenTracing system.
    /// Spans are created by the Tracer interface.
    /// </summary>
    public interface ISpan
    {
        /// <summary>
        /// GetSpanContext() yields the SpanContext for this Span. Note that the 
        /// return value of GetSpanContext() is still valid after a call to 
        /// Span.Finish().
        /// </summary>
        ISpanContext GetSpanContext();

        /// <summary>
        /// Sets the end timestamp and finalizes Span state.
        ///
        /// With the exception of calls to Context() (which are always allowed),
        /// Finish() must be the last call made to any span instance, and to do
        /// otherwise leads to undefined behavior.
        /// </summary>
        void Finish();

        /// <summary>
        /// FinishWithOptions is like Finish() but with explicit control over
	    /// timestamps and log data.
        /// </summary>
        void FinishWithOptions(FinishSpanOptions finishSpanOptions);

        void SetBaggageItem(string restrictedKey, string value);
        string GetBaggageItem(string key);

        /// <summary>
        // Adds a tag to the span.
        //
        // If there is a pre-existing tag set for `key`, it is overwritten.
        /// </summary>
        void SetTag(string message, string value);

        /// <summary>
        // Adds a tag to the span.
        //
        // If there is a pre-existing tag set for `key`, it is overwritten.
        /// </summary>
        void SetTag(string message, int value);

        /// <summary>
        // Adds a tag to the span.
        //
        // If there is a pre-existing tag set for `key`, it is overwritten.
        /// </summary>
        void SetTag(string message, bool value);

        /// <summary>
        /// Adds logdata to the span.
        /// </summary>
        /// <param name="logData"></param>
        void Log(LogData logData);
    }
}