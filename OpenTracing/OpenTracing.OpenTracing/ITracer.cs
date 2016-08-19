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
        ///  Return a new SpanBuilder for a Span with the given `operationName`.
        /// </summary>
        /// <example>
        ///     ITracer tracer = ...
        /// 
        ///     ISpan parentSpan = tracer.BuildSpan("DoWork")
        ///                          .Start();
        /// 
        ///     Span http = tracer.BuildSpan("HandleHTTPRequest")
        ///                        .AsChildOf(parentSpan.context())
        ///                        .WithTag("user_agent", req.UserAgent)
        ///                        .WithTag("lucky_number", 42)
        ///                        .Start();
        /// </example>
        /// <param name="operationName"></param>
        /// <returns>SpanBuilder</returns>
        SpanBuilder BuildSpan(string operationName);


        ISpan StartSpan(StartSpanOptions startSpanOptions);

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
