using OpenTracing.Propagation;

namespace OpenTracing.Carrier.HttpClient
{
    public class HttpClientCarrier : IInjectCarrier
    {
        private ISpanMapper<TextMapFormat> _spanMapper;
        System.Net.Http.HttpClient _httpClient;

        public HttpClientCarrier(ISpanMapper<TextMapFormat> spanMapper, System.Net.Http.HttpClient httpClient)
        {
            _spanMapper = spanMapper;
            _httpClient = httpClient;
        }

        public void MapFrom(ISpan span)
        {
            var textMap = _spanMapper.MapFrom(span);

            var headers = _httpClient.DefaultRequestHeaders;

            foreach(var property in textMap)
            {
                headers.Add(property.Key, property.Value);
            }
        }
    }
}
