﻿namespace OpenTracing.Propagation
{
    /// <summary>
    /// Format instances control the behavior of <see cref="ITracer.Inject{TCarrier}"/> and
    /// <see cref="ITracer.Extract{TCarrier}"/> (and also constrain the type of the carrier parameter to same). Most
    /// OpenTracing users will only reference the <see cref="BuiltinFormats"/> constants. For example:
    /// <code>
    /// Tracer tracer = ...
    /// IFormat{ITextMap} httpCarrier = new AnHttpHeaderCarrier(httpRequest);
    /// ISpanContext spanCtx = tracer.Extract(BuiltinFormats.HttpHeaders, httpHeaderRequest);
    /// </code>
    /// </summary>
    /// <seealso cref="ITracer.Inject{TCarrier}"/>
    /// <seealso cref="ITracer.Extract{TCarrier}"/>
    public interface IFormat<TCarrier>
    {
    }
}
