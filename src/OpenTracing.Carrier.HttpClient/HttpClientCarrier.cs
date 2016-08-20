using OpenTracing.Propagation;

namespace OpenTracing.Carrier.HttpClient
{
    public class HttpClientCarrier : IInjectCarrier<TextMapFormat>
    {
        System.Net.Http.HttpClient _httpClient;

        public HttpClientCarrier(System.Net.Http.HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public void MapFrom(TextMapFormat format)
        {
            var textMap = format;

            var headers = _httpClient.DefaultRequestHeaders;

            foreach (var property in textMap)
            {
                headers.Add(property.Key, property.Value);
            }
        }
    }
}