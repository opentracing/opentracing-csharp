namespace OpenTracing.Propagation
{
    using System.IO;

    public static class BuiltinFormats
    {
        /// <summary>
        ///     The TextMap format allows for arbitrary string-string dictionary encoding of SpanContext state for
        ///     <see cref="ITracer.Inject{T}" /> and <see cref="ITracer.Extract{T}" />.
        ///     Unlike <see cref="HttpHeaders" />, the builtin TextMap format expresses no constraints on keys or values.
        /// </summary>
        /// <seealso cref="ITracer.Inject{T}" />
        /// <seealso cref="ITracer.Extract{T}" />
        /// <seealso cref="IFormat{T}" />
        /// <seealso cref="HttpHeaders" />
        public static readonly IFormat<TextMap> TextMap = new Builtin<TextMap>("TEXT_MAP");

        /// <summary>
        ///     The HttpHeaders format allows for HTTP-header-compatible string-string dictionary encodin of SpanContext state
        ///     for <see cref="ITracer.Inject{T}" /> and <see cref="ITracer.Extract{T}" />.
        ///     I.e, keys written to the TextMap MUST be suitable for HTTP header keys (which are poorly defined but
        ///     certainly restricted); and similarly for values (i.e., URL-escaped and "not too long").
        /// </summary>
        /// <seealso cref="ITracer.Inject{T}" />
        /// <seealso cref="ITracer.Extract{T}" />
        /// <seealso cref="IFormat{T}" />
        /// <seealso cref="TextMap" />
        public static readonly IFormat<TextMap> HttpHeaders = new Builtin<TextMap>("HTTP_HEADERS");

        /// <summary>
        ///     The Binary format allows for unconstrained binary encoding of the SpanContext state for
        ///     <see cref="ITracer.Inject{T}" /> and
        ///     <see cref="ITracer.Extract{T}" />.
        /// </summary>
        /// <seealso cref="ITracer.Inject{T}" />
        /// <seealso cref="ITracer.Extract{T}" />
        /// <seealso cref="IFormat{T}" />
        public static readonly IFormat<Stream> Binary = new Builtin<Stream>("BINARY");

        private sealed class Builtin<T> : IFormat<T>
        {
            private readonly string name;

            public Builtin(string name)
            {
                this.name = name;
            }

            /// <summary>
            ///     Short name for built-in formats as they tend to show up in exception messages
            /// </summary>
            public override string ToString()
            {
                return $"{this.GetType().Name}.{this.name}";
            }
        }
    }
}