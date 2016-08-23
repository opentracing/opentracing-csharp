using System;
using System.Collections.Generic;
using System.Net.Http.Headers;

namespace OpenTracing.Propagation
{
    /// <summary>
    /// A <see cref="ITextMapCarrier"/> which allows <see cref="HttpHeaders"/> implementations to be used as carrier objects.
    /// </summary>
    /// <remarks>
    /// <see cref="HttpHeaders"/> is a multi-value dictionary. Since most other platforms represent http headers as regular
    /// dictionaries, this carrier represents it as a regular dictionary to tracer implementations.</remarks>
    public class HttpHeadersCarrier : ITextMapCarrier
    {
        // TODO should this class be internal/private? It's name is misleading because this only works with the .NET framework class "HttpHeaders"
        // (The headers from ASP.NET Core are not compatible with this class.)

        private readonly HttpHeaders _headers;

        public HttpHeadersCarrier(HttpHeaders headers)
        {
            if (headers == null)
            {
                throw new ArgumentNullException(nameof(headers));
            }

            _headers = headers;
        }

        public IEnumerable<KeyValuePair<string, string>> GetEntries()
        {
            foreach (var kvp in _headers)
            {
                yield return new KeyValuePair<string, string>(kvp.Key, kvp.ToString());
            }
        }

        public string Get(string key)
        {
            IEnumerable<string> values;
            if (_headers.TryGetValues(key, out values))
            {
                // TODO correct behavior?
                return string.Join(",", values);
            }

            return null;
        }

        public void Add(string key, string value)
        {
            _headers.Add(key, value);
        }
    }
}