using System.Collections.Generic;

namespace OpenTracing.Propagation
{
    public interface IContextTextMapMapper<T>
    {
        bool TryMapTo(IReadOnlyDictionary<string, string> data, out T spanContext);
        IReadOnlyDictionary<string, string> MapFrom(T spanContext);
    }
}
