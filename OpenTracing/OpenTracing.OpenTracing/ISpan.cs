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
        /// Sets the end timestamp to DateTime.Noew and finalizes Span state.
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

        /// <summary>
        /// SetBaggageItem sets a key:value pair on this Span and its SpanContext
        /// that also propagates to descendants of this Span.
        ///
        /// SetBaggageItem() enables powerful functionality given a full-stack
        /// opentracing integration (e.g., arbitrary application data from a mobile
        /// app can make it, transparently, all the way into the depths of a storage
        /// system), and with it some powerful costs: use this feature with care.
        ///
        /// IMPORTANT NOTE #1: SetBaggageItem() will only propagate baggage items to
        /// *future* causal descendants of the associated Span.
        ///
        /// IMPORTANT NOTE #2: Use this thoughtfully and with care. Every key and
        /// value is copied into every local *and remote* child of the associated
        /// Span, and that can add up to a lot of network and cpu overhead.
        /// </summary>
        void SetBaggageItem(string restrictedKey, string value);

        /// <summary>
        /// Gets the value for a baggage item given its key. Returns the empty string
        /// if the value isn't found in this Span.
        /// </summary>
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