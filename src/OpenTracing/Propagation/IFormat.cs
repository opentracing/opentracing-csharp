// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFormat.cs">
//   Copyright 2017-2018 The OpenTracing Authors
//   
//   Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except
//   in compliance with the License. You may obtain a copy of the License at
//   
//   http://www.apache.org/licenses/LICENSE-2.0
// 
//   Unless required by applicable law or agreed to in writing, software distributed under the License
//   is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
//   or implied. See the License for the specific language governing permissions and limitations under
//   the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OpenTracing.Propagation
{
    using System.IO;

    /// <summary>
    ///     Format instances control the behavior of <see cref="ITracer.Inject{T}" /> and <see cref="ITracer.Extract{T}" />
    ///     (and also oconstrain the type of the
    ///     carrier parameter to same).
    ///     Most OpenTracing users will only reference the <see cref="BuiltinFormats" /> constants. For example:
    ///     <code>
    /// TODO: This sample looks a bit wonky, missed update at some point?
    /// Tracer tracer = ...
    /// IFormat{TextMap} httpCarrier = new AnHttpHeaderCarrier(httpRequest);
    /// SpanContext spanCtx = tracer.Extract(BuiltinFormats.HttpHeaders, httpHeaderRequest);
    /// </code>
    /// </summary>
    /// <seealso cref="ITracer.Inject{T}" />
    /// <seealso cref="ITracer.Extract{T}" />
    internal interface IFormat<T>
    {
    }

    internal static class BuiltinFormats
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