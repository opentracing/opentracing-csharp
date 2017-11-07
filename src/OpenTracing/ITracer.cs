using OpenTracing.Propagation;
using System;
using System.Collections.Generic;

namespace OpenTracing
{
    /// <summary>
    /// <see cref="ITracer"/> is a simple, thin interface for span creation and propagation across arbitrary transports.
    /// </summary>
    public interface ITracer
    {
        /// <summary>
        /// Starts a new Span
        /// </summary>
        /// <param name="operationName">The operation name of the span.</param>
        ISpan StartSpan(
            string operationName,
            IEnumerable<Tuple<string, ISpanContext>> references = null,
            DateTimeOffset? explicitStartTime = null,
            IEnumerable<KeyValuePair<string, string>> stringTags = null,
            IEnumerable<KeyValuePair<string, bool>> boolTags = null,
            IEnumerable<KeyValuePair<string, double>> doubleTags = null,
            IEnumerable<KeyValuePair<string, int>> intTags = null);

        /// <summary>
        /// Inject a <see cref="ISpanContext"/> into a <paramref name="carrier"/> of a given type,
        /// presumably for propagation across process boundaries.
        /// </summary>
        /// <param name="spanContext">The <see cref="ISpanContext"/> instance to inject into the carrier.</param>
        /// <param name="format">The format of the carrier.</param>
        /// <param name="carrier">See the documentation for the chosen <paramref name="format"/> for a description of the carrier object.</param>
        void Inject<TCarrier>(ISpanContext spanContext, Format<TCarrier> format, TCarrier carrier);

        /// <summary>
        /// Extract a <see cref="ISpanContext"/> from a <paramref name="carrier"/> of a given type,
        /// presumably after propagation across a process boundary.
        /// </summary>
        /// <param name="format">The format of the carrier.</param>
        /// <param name="carrier">See the documentation for the chosen <paramref name="format"/> for a description of the carrier object.</param>
        ISpanContext Extract<TCarrier>(Format<TCarrier> format, TCarrier carrier);
    }
}
