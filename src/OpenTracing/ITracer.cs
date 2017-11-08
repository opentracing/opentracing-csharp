using OpenTracing.Propagation;

namespace OpenTracing
{
    /// <summary>
    /// The <see cref="ITracer"/> interface creates <see cref="ISpan"/>s and understands how to <see cref="Inject{TCarrier}"/>
    /// (serialize) and <see cref="Extract{TCarrier}"/> (deserialize) them across process boundaries.
    /// </summary>
    public interface ITracer
    {
        /// <summary>
        /// Returns a new <see cref="ISpanBuilder" /> for a span with the given <paramref name="operationName" />.
        /// </summary>
        /// <param name="operationName">
        /// An operation name, a human-readable string which concisely represents the work done by the Span (for example, an RPC method name, 
        /// a function name, or the name of a subtask or stage within a larger computation). The operation name should be the most general 
        /// string that identifies a (statistically) interesting class of Span instances. That is, "get_user" is better than "get_user/314159".
        /// <example>
        /// <c>get</c> Too general
        /// <c>get_account/792</c> Too specific
        /// <c>get_account</c> Good, and <c>account_id=792</c> would make a nice <see cref="ISpan"/> Tag.
        /// </example>
        /// <seealso cref="ISpanBuilder.SetOperationName"/>
        /// </param>
        ISpanBuilder BuildSpan(string operationName);

        /// <summary>
        /// Inject a <see cref="ISpanContext"/> into a <paramref name="carrier"/> of a given type,
        /// presumably for propagation across process boundaries.
        /// </summary>
        /// <param name="spanContext">The <see cref="ISpanContext"/> instance to inject into the carrier.</param>
        /// <param name="format">The format of the carrier.</param>
        /// <param name="carrier">See the documentation for the chosen <paramref name="format"/> for a description of the carrier object.</param>
        /// <remarks>
        /// All <see cref="Inject{TCarrier}"/> implementations must support <see cref="Formats.HttpHeaders"/>,
        /// <see cref="Formats.TextMap"/>, and <see cref="Formats.Binary"/>
        /// </remarks>
        /// <exception cref="UnsupportedFormatException">Thrown when the provided format value is unknown or disallowed</exception>
        void Inject<TCarrier>(ISpanContext spanContext, Format<TCarrier> format, TCarrier carrier);

        /// <summary>
        /// Extract a <see cref="ISpanContext"/> from a <paramref name="carrier"/> of a given type,
        /// presumably after propagation across a process boundary.
        /// </summary>
        /// <param name="format">The format of the carrier.</param>
        /// <param name="carrier">
        /// The carrier for the <see cref="ISpanContext"/> state.
        /// See the documentation for the chosen <paramref name="format"/> for a description of the carrier object.
        /// </param>
        /// <remarks>
        /// All <see cref="Extract{TCarrier}"/> implementations must support <see cref="Formats.HttpHeaders"/>,
        /// <see cref="Formats.TextMap"/>, and <see cref="Formats.Binary"/>
        /// </remarks>
        /// <exception cref="UnsupportedFormatException">Thrown when the provided format value is unknown or disallowed</exception>
        ISpanContext Extract<TCarrier>(Format<TCarrier> format, TCarrier carrier);
    }
}
