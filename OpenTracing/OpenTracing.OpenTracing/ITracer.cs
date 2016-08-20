using OpenTracing.Propagation;
using System;

namespace OpenTracing
{
    /// <summary>
    /// Tracer is a simple, thin interface for Span creation and propagation across arbitrary transports.
    /// </summary>
    public interface ITracer
    {
        /// <summary>
        /// Create, start, and return a new Span.
        /// 
        /// A Span with no SpanReference options becomes the root of its own trace.
        /// </summary>
        /// <param name="operationName"></param>
        /// <param name="startSpanOptions"></param>
        /// <returns></returns>
        ISpan StartSpan(string operationName, StartSpanOptions startSpanOptions);

        /// <summary>
        /// Inject a SpanContext into a `carrier` of a given type, presumably for propagation across process boundaries.
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
        /// Extract a SpanContext from a `carrier` of a given type, presumably after propagation across a process boundary.
        /// </summary>
        /// <typeparam name="TFormat">The Format of the carrier</typeparam>
        /// <param name="carrier">The carrier type, which also parametrizes the TFormat.</param>
        /// <example>
        /// <code>
        /// Tracer tracer = ...
        /// MemoryTextMapCarrier memoryCarrier = new MemoryTextMapCarrier();
        /// ExtractResult extractResult = tracer.extract(memoryCarrier);
        /// tracer.BuildSpan('...').withChildOf(spanCtx.SpanContext).start();
        /// </code>
        /// </example>
        /// <returns>The SpanContext instance holding context to create a Span.</returns>
        ExtractResult Extract<TFormat>(string operationName, IExtractCarrier<TFormat> carrier);
    }
}
