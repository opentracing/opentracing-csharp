using System.Collections.Generic;

namespace OpenTracing.Propagation
{
    public class MemoryTextMapCarrier<T> : IInjectCarrier<T>, IExtractCarrier<T>
    {
        private IContextTextMapMapper<T> _contextMapper;

        public IReadOnlyDictionary<string, string> TextMap { get; set; }

        public MemoryTextMapCarrier(IContextTextMapMapper<T> contextMapper, IReadOnlyDictionary<string, string> textMap)
        {
            TextMap = textMap;
            _contextMapper = contextMapper;
        }

        public bool TryMapTo(out T spanContext)
        {
            return _contextMapper.TryMapTo(TextMap, out spanContext);
        }

        public void MapFrom(T spanContext)
        {
            TextMap = _contextMapper.MapFrom(spanContext);
        }
    }
}