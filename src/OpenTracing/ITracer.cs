using OpenTracing.Propagation;

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
        /// <typeparam name="TFormat">The Format of the carrier</typeparam>
        /// <param name="spanContext">The SpanContext instance to inject into the carrier</param>
        /// <param name="carrier">The carrier type, which also parametrizes the TFormat.</param>
        /// <example>
        /// <code>ITracer tracer = ...
        /// ISpan clientSpan = ...
        /// 
        /// MemoryTextMapCarrier memoryCarrier = new MemoryTextMapCarrier();
        /// tracer.Inject(span, memoryCarrier);
        /// </code>
        /// </example>
        void Inject<TFormat>(ISpanContext spanContext, IInjectCarrier<TFormat> carrier);

        /// <summary>
        /// Returns a new <see cref="ISpanContext"/> containing the baggage of the given <paramref name="carrier"/>,
        /// or null if there was no baggage found.
        /// </summary>
        /// <typeparam name="TFormat">The Format of the carrier</typeparam>
        /// <param name="carrier">The carrier type, which also parametrizes the TFormat.</param>
        /// <example>
        /// <code>
        ///     Tracer tracer = ...
        ///     MemoryTextMapCarrier memoryCarrier = new MemoryTextMapCarrier();
        ///     ExtractResult extractResult = tracer.extract(memoryCarrier);
        ///     tracer.BuildSpan('...').withChildOf(spanCtx.SpanContext).start();
        /// </code>
        /// </example>
        ISpanContext Extract<TFormat>(IExtractCarrier<TFormat> carrier);
    }
}
