using OpenTracing.Propagation;

namespace OpenTracing.Carrier.HttpClient
{
    public class HttpClientCarrier<T> : IInjectCarrier<T>
    {
        private IContextTextMapMapper<T> _contextMapper;
        private System.Net.Http.HttpClient _httpClient;

        public HttpClientCarrier(IContextTextMapMapper<T> contextMapper, System.Net.Http.HttpClient httpClient)
        {
            _contextMapper = contextMapper;
            _httpClient = httpClient;
        }

        public void MapFrom(T spanContext)
        {
            var textMap = _contextMapper.MapFrom(spanContext);

            var headers = _httpClient.DefaultRequestHeaders;

            foreach(var property in textMap)
            {
                headers.Add(property.Key, property.Value);
            }
        }
    }
}
