namespace OpenTracing.Propagation
{
    public static class BuiltinFormats
    {
        /// <summary>
        /// The ITextMap format allows for arbitrary string-string dictionary encoding of SpanContext state for
        /// <see cref="ITracer.Inject{TCarrier}"/> and <see cref="ITracer.Extract{TCarrier}"/>. Unlike <see cref="HttpHeaders"/>,
        /// the builtin ITextMap format expresses no constraints on keys or values.
        /// </summary>
        /// <seealso cref="ITracer.Inject{TCarrier}"/>
        /// <seealso cref="ITracer.Extract{TCarrier}"/>
        /// <seealso cref="Format{TCarrier}"/>
        /// <seealso cref="HttpHeaders"/>
        public static readonly Format<ITextMap> TextMap = new Format<ITextMap>("TEXT_MAP");

        /// <summary>
        /// The HttpHeaders format allows for HTTP-header-compatible string-string dictionary encodin of SpanContext state
        /// for <see cref="ITracer.Inject{TCarrier}"/> and <see cref="ITracer.Extract{TCarrier}"/>. I.e, keys written to the
        /// ITextMap MUST be suitable for HTTP header keys (which are poorly defined but certainly restricted); and similarly for
        /// values (i.e., URL-escaped and "not too long").
        /// </summary>
        /// <seealso cref="ITracer.Inject{TCarrier}"/>
        /// <seealso cref="ITracer.Extract{TCarrier}"/>
        /// <seealso cref="Format{TCarrier}"/>
        /// <seealso cref="TextMap"/>
        public static readonly Format<ITextMap> HttpHeaders = new Format<ITextMap>("HTTP_HEADERS");
    }
}