using System;
using System.IO;
using OpenTracing.Propagation;

namespace OpenTracing
{
    /// <summary>Tracer is a simple, thin interface for Span creation and propagation across arbitrary transports.</summary>
    public interface ITracer
    {
        /// <summary>The current <see cref="IScopeManager"/>, which may be a noop but may not be null.</summary>
        IScopeManager ScopeManager { get; }

        /// <summary>
        /// Get the active <see cref="ISpan"/>. This is a shorthand for <code>ITracer.ScopeManager.Active.Span</code>,
        /// and null will be returned if <see cref="IScopeManager.Active"/> is null.
        /// </summary>
        ISpan ActiveSpan { get; }

        /// <summary>
        /// Return a new SpanBuilder for a Span with the given 'operationName'.
        /// <para>You can override the operationName later via <see cref="ISpan.SetOperationName"/>.</para>
        /// <para>
        /// A contrived example:
        /// <code>
        /// ITracer tracer = ...
        ///
        /// // Note if there is a ITracer.ActiveSpan(), it will be used as the target of an implicit CHILD_OF
        /// // Refernece for "workSpan" when StartActive() is invoked.
        /// using (IActiveSpan workSpan = tracer.BuildSpan("DoWork").StartActive())
        /// {
        ///     workSpan.SetTag("...", "...");
        ///     // etc, etc
        /// }
        ///
        /// // It's also possible to create Spans manually, bypassing the ActiveSpanSource activation.
        /// Span http = tracer.BuildSpan("HandleHTTPRequest")
        ///                   .AsChildOf(rpcSpanContext)  // an explicit parent
        ///                   .WithTag("user_agent", req.UserAgent)
        ///                   .WithTag("lucky_number", 42)
        ///                   .StartManual();
        ///         </code>
        /// </para>
        /// </summary>
        /// <param name="operationName"></param>
        ISpanBuilder BuildSpan(string operationName);

        /// <summary>
        /// Inect a SpanContext into a 'carrier' of a given type, presumably for propagation across process boundaries.
        /// <para>
        /// Example:
        /// <code>
        /// Tracer tracer = ...
        /// Span clientSpan = ...
        /// ITextMap httpHeadersCarrier = new AnHttpHeaderCarrier(httpRequest);
        /// tracer.Inject(span.Context(), BuiltinFormats.HttpHeaders, httpHeadersCarrier);
        /// </code>
        /// </para>
        /// </summary>
        /// <typeparam name="TCarrier">The carrier type, which also parametrizes the Format.</typeparam>
        /// <param name="spanContext">The SpanContext instance to inject into the carrier</param>
        /// <param name="format">The Fromat of the carrier</param>
        /// <param name="carrier">
        /// The carrier for the SpanContext state. All Tracer.Inject implementations must support
        /// <see cref="ITextMap"/> and <see cref="Stream"/>
        /// </param>
        /// <seealso cref="IFormat{TCarrier}"/>
        /// <seealso cref="BuiltinFormats"/>
        void Inject<TCarrier>(ISpanContext spanContext, IFormat<TCarrier> format, TCarrier carrier);

        /// <summary>
        /// Extract a SpanContext from a 'carrier' of a given type, presumably after propagation across a process boundary.
        /// <para>
        /// Example:
        /// <code>
        /// Tracer tracer = ...
        /// ITextMap httpHeadersCarrier = new AnHttpHeaderCarrier(httpRequest);
        /// SpanContext spanCtx = tracer.Extract(BuiltinFormats.HttpHeaders, httpHeadersCarrier);
        /// ... = tracer.BuildSpan("...").AsChildOf(spanCtx).StartActive();
        /// </code>
        /// </para>
        /// If the span serialized state is invalid (corrupt, wrong version, etc) inside the carrier this will result in an
        /// <see cref="ArgumentException"/>.
        /// </summary>
        /// <exception cref="ArgumentException">If the span serialized state is invalid (corrupt, wrong version, etc)</exception>
        /// <typeparam name="TCarrier">The carrier type, which also parametrizes the Format.</typeparam>
        /// <param name="format">The Format of the carrier</param>
        /// <param name="carrier">
        /// The carrier for the SpanContext state. All Tracer.Extract() implementations must support
        /// <see cref="ITextMap"/> and <see cref="Stream"/>.
        /// </param>
        /// <returns>The SpanContext instance holding context to create a Span.</returns>
        /// <seealso cref="IFormat{TCarrier}"/>
        /// <seealso cref="BuiltinFormats"/>
        ISpanContext Extract<TCarrier>(IFormat<TCarrier> format, TCarrier carrier);
    }
}
