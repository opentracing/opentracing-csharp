namespace OpenTracing.Propagation
{
    /// <summary>
    /// Format instances control the behavior of <see cref="ITracer.Inject{T}"/> and <see cref="ITracer.Extract{T}"/>
    /// (and also constrain the type of the
    /// carrier parameter to same).
    /// Most OpenTracing users will only reference the <see cref="BuiltinFormats"/> constants. For example:
    /// <code>
    /// Tracer tracer = ...
    /// IFormat{TextMap} httpCarrier = new AnHttpHeaderCarrier(httpRequest);
    /// ISpanContext spanCtx = tracer.Extract(BuiltinFormats.HttpHeaders, httpHeaderRequest);
    /// </code>
    /// </summary>
    /// <seealso cref="ITracer.Inject{T}"/>
    /// <seealso cref="ITracer.Extract{T}"/>
    public interface IFormat<T>
    {
    }
}