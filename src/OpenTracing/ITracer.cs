﻿using OpenTracing.Propagation;

namespace OpenTracing
{
    public interface ITracer
    {
        /// <summary>
        /// Returns a new <see cref="ISpanBuilder" /> for a Span with the given <paramref name="operationName" />.
        /// </summary>
        /// <param name="operationName">The operation name of the Span.</param>
        ISpanBuilder BuildSpan(string operationName);

        /// <summary>
        /// This takes the <paramref name="spanContext"/> and injects it for propagation within <paramref name="carrier"/>. 
        /// The actual type of <paramref name="carrier"/> depends on the value of <paramref name="format"/>.
        /// </summary>
        /// <param name="spanContext">The <see cref="ISpanContext"/> instance to inject into the carrier.</param>
        /// <param name="format">The format which should be used.</param>
        /// <param name="carrier">See the documentation for the chosen <paramref name="format"/> for a description of the carrier object.</param>
        void Inject<TCarrier>(ISpanContext spanContext, Format<TCarrier> format, TCarrier carrier);

        /// <summary>
        /// Returns a new <see cref="ISpanContext"/> containing the baggage of the given <paramref name="carrier"/>,
        /// or null if there was no baggage found.
        /// </summary>
        /// <param name="format">The format which should be used.</param>
        /// <param name="carrier">See the documentation for the chosen <paramref name="format"/> for a description of the carrier object</param>
        ISpanContext Extract<TCarrier>(Format<TCarrier> format, TCarrier carrier);
    }
}
