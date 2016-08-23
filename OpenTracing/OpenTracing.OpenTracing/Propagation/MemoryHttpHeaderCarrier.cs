using System.Collections.Generic;

namespace OpenTracing.Propagation
{
    /// <summary>
    /// MemoryHttpHeaderCarrier is a built-in carrier for Tracer.Inject() and 
    /// Tracer.Extract() using the HttpHeaderFormat format.
    /// </summary>
    public class MemoryHttpHeaderCarrier : IInjectCarrier<HttpHeaderFormat>, IExtractCarrier<HttpHeaderFormat>
    {
        public IDictionary<string, string> TextMap { get; set; } = new Dictionary<string, string>() { };

        public MemoryHttpHeaderCarrier()
        {
        }

        public MemoryHttpHeaderCarrier(IDictionary<string, string> textMap)
        {
            TextMap = textMap;
        }

        /// <summary>
        /// MapFrom takes the SpanContext instance in a HttpHeaderFormat and injects it 
        /// for propagation within the MemoryHttpHeaderCarrier. 
        /// </summary>
        public void MapFrom(HttpHeaderFormat context)
        {
            TextMap = context;
        }

        /// <summary>
        /// Extract returns the SpanContext propagated through the MemoryHttpHeaderCarrier
        /// in a HttpHeaderFormat.
        /// </summary>
        public ExtractCarrierResult<HttpHeaderFormat> Extract()
        {
            return new ExtractCarrierResult<HttpHeaderFormat>(new HttpHeaderFormat(TextMap));
        }
    }
}