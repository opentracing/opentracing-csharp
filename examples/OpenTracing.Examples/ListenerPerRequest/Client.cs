using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OpenTracing.Tag;

namespace OpenTracing.Examples.ListenerPerRequest
{
    public class Client
    {
        private readonly ITracer tracer;

        public Client(ITracer tracer)
        {
            this.tracer = tracer;
        }

        // Async execution
        private Task<String> Execute(String message, ResponseListener responseListener)
        {
            return Task.Run(() =>
            {
                // send via wire and get response
                String response = $"{message}:response";
                responseListener.OnResponse(response);
                return response;
            });
        }

        public Task<String> Send(String message)
        {
            ISpan span = tracer.BuildSpan("send").
                    WithTag(Tags.SpanKind.Key, Tags.SpanKindClient)
                    .Start();
            return Execute(message, new ResponseListener(span));
        }
    }
}