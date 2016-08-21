using System;
using System.Collections.Generic;

namespace OpenTracing.Propagation
{
    public class HttpHeadersCarrier : IInjectCarrier, IExtractCarrier
    {
        public IEnumerable<KeyValuePair<string, IEnumerable<string>>> Headers { get; }

        public HttpHeadersCarrier(IEnumerable<KeyValuePair<string, IEnumerable<string>>> headers)
        {
            if (headers == null)
            {
                throw new ArgumentNullException(nameof(headers));
            }

            Headers = headers;
        }
    }
}