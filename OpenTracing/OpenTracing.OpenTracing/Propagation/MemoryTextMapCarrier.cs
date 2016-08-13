using System.Collections.Generic;

namespace OpenTracing.Propagation
{
    public class MemoryTextMapCarrier : IInjectCarrier<TextMapFormat>, IExtractCarrier<TextMapFormat>
    {
        public IDictionary<string, string> TextMap { get; set; } = new Dictionary<string, string>() { };

        public MemoryTextMapCarrier()
        {
        }

        public MemoryTextMapCarrier(IDictionary<string, string> textMap)
        {
            TextMap = textMap;
        }   

        public void MapFrom(TextMapFormat format)
        {
            TextMap = format;
        }

        public ExtractCarrierResult<TextMapFormat> Extract()
        {
            return new ExtractCarrierResult<TextMapFormat>(new TextMapFormat(TextMap));
        }
    }
}