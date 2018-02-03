namespace OpenTracing.Propagation
{
    public static class BuiltinFormats
    {
        /// <summary>
        /// The TextMap format allows for arbitrary string-string dictionary encoding of SpanContext state for
        /// <see cref="ITracer.Inject{TCarrier}"/> and <see cref="ITracer.Extract{TCarrier}"/>. Unlike <see cref="HttpHeaders"/>,
        /// the builtin TextMap format expresses no constraints on keys or values.
        /// </summary>
        /// <seealso cref="ITracer.Inject{TCarrier}"/>
        /// <seealso cref="ITracer.Extract{TCarrier}"/>
        /// <seealso cref="IFormat{TCarrier}"/>
        /// <seealso cref="HttpHeaders"/>
        public static readonly IFormat<TextMap> TextMap = new Builtin<TextMap>("TEXT_MAP");

        /// <summary>
        /// The HttpHeaders format allows for HTTP-header-compatible string-string dictionary encodin of SpanContext state
        /// for <see cref="ITracer.Inject{TCarrier}"/> and <see cref="ITracer.Extract{TCarrier}"/>. I.e, keys written to the
        /// TextMap MUST be suitable for HTTP header keys (which are poorly defined but certainly restricted); and similarly for
        /// values (i.e., URL-escaped and "not too long").
        /// </summary>
        /// <seealso cref="ITracer.Inject{TCarrier}"/>
        /// <seealso cref="ITracer.Extract{TCarrier}"/>
        /// <seealso cref="IFormat{TCarrier}"/>
        /// <seealso cref="TextMap"/>
        public static readonly IFormat<TextMap> HttpHeaders = new Builtin<TextMap>("HTTP_HEADERS");

        private struct Builtin<TCarrier> : IFormat<TCarrier>
        {
            private readonly string _name;

            public Builtin(string name)
            {
                _name = name;
            }

            /// <summary>Short name for built-in formats as they tend to show up in exception messages</summary>
            public override string ToString()
            {
                return $"{GetType().Name}.{_name}";
            }
        }
    }
}