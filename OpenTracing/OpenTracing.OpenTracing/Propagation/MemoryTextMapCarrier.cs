using System.Collections.Generic;

namespace OpenTracing.Propagation
{
    public class MemoryTextMapCarrier : IInjectCarrier, IExtractCarrier
    {
        private ISpanMapper<TextMapFormat> _contextMapper;

        public IDictionary<string, string> TextMap { get; set; } = new Dictionary<string, string>() { };

        public MemoryTextMapCarrier(ISpanMapper<TextMapFormat> contextMapper, IDictionary<string, string> textMap)
        {
            TextMap = textMap;
            _contextMapper = contextMapper;
        }

        public bool TryMapTo(string operationName, out ISpan span)
        {
            return _contextMapper.TryMapTo(operationName, new TextMapFormat(TextMap), out span);
        }

        public void MapFrom(ISpan span)
        {
            TextMap = _contextMapper.MapFrom(span);
        }
    }
}