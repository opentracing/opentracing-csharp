using System;
using System.Collections.Generic;

namespace OpenTracing
{
    /// <summary>
    /// Provides a way to configure the behavior of <see cref="ITracer.StartSpan"/>.
    /// </summary>
    public class StartSpanOptions
    {
        // TODO should References/Tags be public set? If yes, should they have an initial value?

        /// <summary>
        /// A list of references to other <see cref="ISpanContext"/>s.
        /// </summary>
        public IList<SpanReference> References { get; set; } = new List<SpanReference>();

        /// <summary>
        /// A list of tags that should be used for the new Span.
        /// </summary>
        public IDictionary<string, object> Tags { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// The start timestamp that should be used for the new Span.
        /// </summary>
        public DateTimeOffset? StartTimestamp { get; set; }
    }
}