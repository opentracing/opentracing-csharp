using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OpenTracing.Examples.CommonRequestHandler
{
    public class Client
    {
        private readonly RequestHandler requestHandler;

        public Client(RequestHandler requestHandler)
        {
            this.requestHandler = requestHandler;
        }

        public async Task<String> Send(String message)
        {
            var context = new Context();

            await Task.Run(async () =>
            {
                await Task.Delay(50);
                requestHandler.BeforeRequest(message, context);
            });

            await Task.Run(async () =>
            {
                await Task.Delay(50);
                requestHandler.AfterResponse(message, context);
            });

            return $"{message}:response";
        }
    }
}