using System;
using System.Net.Http.Headers;

namespace OpenTracing.Propagation
{
    public class HttpHeadersCarrier : IInjectCarrier, IExtractCarrier
    {
        public HttpHeaders Headers { get; }

        public HttpHeadersCarrier(HttpHeaders headers)
        {
            if (headers == null)
            {
                throw new ArgumentNullException(nameof(headers));
            }

            Headers = headers;
        }
    }
}