using System;
using System.IO;

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

        /// <summary>
        /// The Binary format allows for unconstrained binary encoding of the SpanContext state for
        /// <see cref="ITracer.Inject{TCarrier}"/> and <see cref="ITracer.Extract{TCarrier}"/>.
        /// </summary>
        /// <seealso cref="ITracer.Inject{TCarrier}"/>
        /// <seealso cref="ITracer.Extract{TCarrier}"/>
        /// <seealso cref="IFormat{TCarrier}"/>
        public static readonly IFormat<Stream> Binary = new Builtin<Stream>("BINARY");

        private struct Builtin<TCarrier> : IFormat<TCarrier>, IEquatable<Builtin<TCarrier>>
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

            public bool Equals(Builtin<TCarrier> other)
            {
                return string.Equals(_name, other._name);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj))
                    return false;
                return obj is Builtin<TCarrier> && Equals((Builtin<TCarrier>) obj);
            }

            public override int GetHashCode()
            {
                return _name != null ? _name.GetHashCode() : 0;
            }

            public static bool operator ==(Builtin<TCarrier> left, Builtin<TCarrier> right)
            {
                return left.Equals(right);
            }

            public static bool operator !=(Builtin<TCarrier> left, Builtin<TCarrier> right)
            {
                return !left.Equals(right);
            }
        }
    }
}