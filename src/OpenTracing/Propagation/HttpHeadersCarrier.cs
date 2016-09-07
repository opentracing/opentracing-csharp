using System;
using System.Collections.Generic;
using System.Net.Http.Headers;

namespace OpenTracing.Propagation
{
    /// <summary>
    /// MemoryHttpHeaderCarrier is a built-in carrier for Tracer.Inject() and 
    /// Tracer.Extract() using the HttpHeaderFormat format.
    /// </summary>
    /// <remarks>
    /// <see cref="HttpHeaders"/> is a multi-value dictionary. Since most other platforms represent http headers as regular
    /// dictionaries, this carrier represents it as a regular dictionary to tracer implementations.</remarks>
    public class HttpHeadersCarrier : IInjectCarrier<HttpHeaderFormat>, IExtractCarrier<HttpHeaderFormat>
    {
        // TODO should this class be internal/private? It's name is misleading because this only works with the .NET framework class "HttpHeaders"
        // (The headers from ASP.NET Core are not compatible with this class.)

        private IDictionary<string, string> _textMap { get; set; } = new Dictionary<string, string>() { };
        public IEnumerable<KeyValuePair<string, string>> TextMap => _textMap;

        public HttpHeadersCarrier(HttpHeaders headers)
        {
            if (headers == null)
            {
                throw new ArgumentNullException(nameof(headers));
            }

            foreach (var kvp in headers)
            {
                _textMap.Add(kvp.Key, string.Join(",", kvp.Value));
            }
        }

        /// <summary>
        /// MapFrom takes the SpanContext instance in a HttpHeaderFormat and injects it 
        /// for propagation within the MemoryHttpHeaderCarrier. 
        /// </summary>
        public void MapFrom(HttpHeaderFormat context)
        {
            _textMap = context;
        }

        /// <summary>
        /// Extract returns the SpanContext propagated through the MemoryHttpHeaderCarrier
        /// in a HttpHeaderFormat.
        /// </summary>
        public HttpHeaderFormat Extract()
        {
            return new HttpHeaderFormat(_textMap);
        }
    }
}