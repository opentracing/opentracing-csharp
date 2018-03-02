using System;
using System.IO;
using OpenTracing.Propagation;

namespace OpenTracing
{
    /// <summary>
    /// <see cref="ITracer"/> is a simple, thin interface for span creation and propagation across arbitrary transports.
    /// </summary>
    public interface ITracer
    {
        /// <summary>The current <see cref="IScopeManager"/>, which may be a noop but may not be null.</summary>
        IScopeManager ScopeManager { get; }

        /// <summary>
        /// Get the active <see cref="ISpan"/>. This is a shorthand for <code>tracer.ScopeManager.Active.Span</code>,
        /// and null will be returned if <see cref="IScopeManager.Active"/> is null.
        /// </summary>
        ISpan ActiveSpan { get; }

        /// <summary>
        /// Returns a new <see cref="ISpanBuilder"/> for a span with the given 'operationName'.
        /// <para>You can override the operationName later via <see cref="ISpan.SetOperationName"/>.</para>
        /// <para>
        /// A contrived example:
        /// <code>
        /// ITracer tracer = ...
        ///
        /// // Note if there is a tracer.ActiveSpan, it will be used as the target of an implicit CHILD_OF
        /// // reference for "workScope.Span" when StartActive() is invoked.
        /// using (IScope workScope = tracer.BuildSpan("DoWork").StartActive(finishSpanOnDispose: true))
        /// {
        ///     workScope.Span.SetTag("...", "...");
        ///     // etc, etc
        /// }
        ///
        /// // It's also possible to create spans manually, bypassing the IScopeManager activation.
        /// ISpan http = tracer.BuildSpan("HandleHTTPRequest")
        ///                   .AsChildOf(rpcSpanContext)  // an explicit parent
        ///                   .WithTag("user_agent", req.UserAgent)
        ///                   .WithTag("lucky_number", 42)
        ///                   .Start();
        ///         </code>
        /// </para>
        /// </summary>
        /// <param name="operationName">Sets the string name for the logical operation the span being built represents.</param>
        ISpanBuilder BuildSpan(string operationName);

        /// <summary>
        /// Inect a <see cref="ISpanContext"/> into a 'carrier' of a given type, presumably for propagation across process boundaries.
        /// <para>
        /// Example:
        /// <code>
        /// ITracer tracer = ...
        /// ISpan clientSpan = ...
        /// ITextMap httpHeadersCarrier = new AnHttpHeaderCarrier(httpRequest);
        /// tracer.Inject(span.Context, BuiltinFormats.HttpHeaders, httpHeadersCarrier);
        /// </code>
        /// </para>
        /// </summary>
        /// <typeparam name="TCarrier">The <paramref name="carrier"/> type, which also parametrizes the <paramref name="format"/>.</typeparam>
        /// <param name="spanContext">The <see cref="ISpanContext"/> instance to inject into the <paramref name="carrier"/>.</param>
        /// <param name="format">The <see cref="IFormat{TCarrier}"/> of the <paramref name="carrier"/>.</param>
        /// <param name="carrier">
        /// The carrier for the <see cref="ISpanContext"/> state. All <see cref="Inject"/> implementations must support
        /// <see cref="ITextMap"/> and <see cref="Stream"/>.
        /// </param>
        /// <seealso cref="IFormat{TCarrier}"/>
        /// <seealso cref="BuiltinFormats"/>
        void Inject<TCarrier>(ISpanContext spanContext, IFormat<TCarrier> format, TCarrier carrier);

        /// <summary>
        /// Extract a <see cref="ISpanContext"/> from a <paramref name="carrier"/> of a given type,
        /// presumably after propagation across a process boundary.
        /// <para>
        /// Example:
        /// <code>
        /// ITracer tracer = ...
        /// ITextMap httpHeadersCarrier = new AnHttpHeaderCarrier(httpRequest);
        /// ISpanContext spanContext = tracer.Extract(BuiltinFormats.HttpHeaders, httpHeadersCarrier);
        /// ... = tracer.BuildSpan("...").AsChildOf(spanContext).StartActive(true);
        /// </code>
        /// </para>
        /// If the span serialized state is invalid (corrupt, wrong version, etc) inside the carrier this will result in an
        /// <see cref="ArgumentException"/>.
        /// </summary>
        /// <exception cref="ArgumentException">If the span serialized state is invalid (corrupt, wrong version, etc).</exception>
        /// <typeparam name="TCarrier">The <paramref name="carrier"/> type, which also parametrizes the <paramref name="format"/>.</typeparam>
        /// <param name="format">The <see cref="IFormat{TCarrier}"/> of the <paramref name="carrier"/>.</param>
        /// <param name="carrier">
        /// The carrier for the <see cref="ISpanContext"/> state. All <see cref="Extract"/> implementations must support
        /// <see cref="ITextMap"/> and <see cref="Stream"/>.
        /// </param>
        /// <returns>The <see cref="ISpanContext"/> instance holding context to create a span.</returns>
        /// <seealso cref="IFormat{TCarrier}"/>
        /// <seealso cref="BuiltinFormats"/>
        ISpanContext Extract<TCarrier>(IFormat<TCarrier> format, TCarrier carrier);
    }
}
