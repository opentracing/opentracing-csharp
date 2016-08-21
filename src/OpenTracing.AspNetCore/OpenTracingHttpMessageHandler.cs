using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace OpenTracing.AspNetCore
{
    public class OpenTracingHttpMessageHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OpenTracingHttpMessageHandler(HttpMessageHandler inner, IHttpContextAccessor httpContextAccessor)
            : base(inner)
        {
            if (httpContextAccessor == null)
            {
                throw new ArgumentNullException(nameof(httpContextAccessor));
            }

            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var tracer = (ITracer)httpContext.RequestServices.GetService(typeof(ITracer));

            ISpan requestSpan = (ISpan)httpContext.Items[typeof(ISpan)];
            ISpan callSpan = null;

            try
            {
                // TODO operationName?
                callSpan = tracer.StartSpan("HttpClient", SpanReference.ChildOf(requestSpan))
                    .SetTagComponent("HttpClient")
                    .SetTagSpanKindClient()
                    .SetTagHttpMethod(request.Method)
                    .SetTagHttpUrl(request.RequestUri);

                // TODO other tags (peer)

                var response = await base.SendAsync(request, cancellationToken);

                callSpan?.SetTagHttpStatusCode(response.StatusCode);

                return response;
            }
            catch
            {
                // TODO log exception details?
                callSpan?.SetTagError();
                throw;
            }
            finally
            {
                callSpan?.Finish(); // TODO only call dispose?
                callSpan?.Dispose();
            }
        }
    }
}