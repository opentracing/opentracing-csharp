using System.Collections.Generic;

namespace OpenTracing.Propagation
{
    /// <summary>
    /// MemoryTextMapCarrier is a built-in carrier for Tracer.Inject() and 
    /// Tracer.Extract() using the TextMap format.
    /// </summary>
    public class DictionaryCarrier : IInjectCarrier<TextMapFormat>, IExtractCarrier<TextMapFormat>
    {
        public IDictionary<string, string> TextMap { get; set; } = new Dictionary<string, string>() { };

        public DictionaryCarrier()
        {
        }

        public DictionaryCarrier(IDictionary<string, string> textMap)
        {
            TextMap = textMap;
        }

        /// <summary>
        /// MapFrom takes the SpanContext instance in a TextMapFormat and injects it 
        /// for propagation within the MemoryTextMapCarrier. 
        /// </summary>
        public void MapFrom(TextMapFormat context)
        {
            TextMap = context;
        }

        /// <summary>
        /// Extract returns the SpanContext propagated through the MemoryTextMapCarrier
        /// in a TextMapFormat.
        /// </summary>
        public TextMapFormat Extract()
        {
            return new TextMapFormat(TextMap);
        }
    }
}