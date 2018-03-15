using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OpenTracing.Tag;

namespace OpenTracing.Examples.ListenerPerRequest
{
    public class Client
    {
        private readonly ITracer _tracer;

        public Client(ITracer tracer)
        {
            _tracer = tracer;
        }

        // Async execution
        private async Task<string> Execute(string message, ResponseListener responseListener)
        {
            await Task.Delay(10);

            // send via wire and get response
            string response = $"{message}:response";
            responseListener.OnResponse(response);
            return response;
        }

        public Task<string> Send(string message)
        {
            ISpan span = _tracer.BuildSpan("send")
                    .WithTag(Tags.SpanKind.Key, Tags.SpanKindClient)
                    .Start();
            return Execute(message, new ResponseListener(span));
        }
    }
}
