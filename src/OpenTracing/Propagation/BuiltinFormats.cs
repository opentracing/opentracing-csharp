using System.IO;

namespace OpenTracing.Propagation
{
    public static class BuiltinFormats
    {
        /// <summary>
        /// The 'TextMap' format allows for arbitrary string-string dictionary encoding of <see cref="ISpanContext"/> state for
        /// <see cref="ITracer.Inject{TCarrier}"/> and <see cref="ITracer.Extract{TCarrier}"/>. Unlike <see cref="HttpHeaders"/>,
        /// the builtin 'TextMap' format expresses no constraints on keys or values.
        /// </summary>
        /// <seealso cref="ITracer.Inject{TCarrier}"/>
        /// <seealso cref="ITracer.Extract{TCarrier}"/>
        /// <seealso cref="IFormat{TCarrier}"/>
        /// <seealso cref="HttpHeaders"/>
        public static readonly IFormat<ITextMap> TextMap = new Builtin<ITextMap>("TEXT_MAP");

        /// <summary>
        /// The 'HttpHeaders' format allows for HTTP-header-compatible string-string dictionary encoding of <see cref="ISpanContext"/> state
        /// for <see cref="ITracer.Inject{TCarrier}"/> and <see cref="ITracer.Extract{TCarrier}"/>. I.e, keys written to the
        /// <see cref="ITextMap"/> MUST be suitable for HTTP header keys (which are poorly defined but certainly restricted); and similarly for
        /// values (i.e., URL-escaped and "not too long").
        /// </summary>
        /// <seealso cref="ITracer.Inject{TCarrier}"/>
        /// <seealso cref="ITracer.Extract{TCarrier}"/>
        /// <seealso cref="IFormat{TCarrier}"/>
        /// <seealso cref="TextMap"/>
        public static readonly IFormat<ITextMap> HttpHeaders = new Builtin<ITextMap>("HTTP_HEADERS");

        /// <summary>
        /// The 'Binary' format allows for unconstrained byte encoding of <see cref="ISpanContext"/> state
        /// for <see cref="ITracer.Inject{TCarrier}"/> and <see cref="ITracer.Extract{TCarrier}"/> using a <see cref="MemoryStream"/>.
        /// </summary>
        /// <seealso cref="ITracer.Inject{TCarrier}"/>
        /// <seealso cref="ITracer.Extract{TCarrier}"/>
        /// <seealso cref="IFormat{TCarrier}"/>
        /// <seealso cref="byte"/>
        public static readonly IFormat<IBinary> Binary = new Builtin<IBinary>("BINARY");

        private struct Builtin<TCarrier> : IFormat<TCarrier>
        {
            private readonly string _name;

            public Builtin(string name)
            {
                _name = name;
            }

            /// <summary>Short name for built-in formats as they tend to show up in exception messages.</summary>
            public override string ToString()
            {
                return $"{GetType().Name}.{_name}";
            }
        }
    }
}
