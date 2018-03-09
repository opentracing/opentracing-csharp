using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OpenTracing.Examples.CommonRequestHandler
{
    public class Client
    {
        private readonly RequestHandler _requestHandler;

        public Client(RequestHandler requestHandler)
        {
            _requestHandler = requestHandler;
        }

        public async Task<string> Send(string message)
        {
            var context = new Context();

            await Task.Run(async () =>
            {
                await Task.Delay(50);
                _requestHandler.BeforeRequest(message, context);
            });

            await Task.Run(async () =>
            {
                await Task.Delay(50);
                _requestHandler.AfterResponse(message, context);
            });

            return $"{message}:response";
        }
    }
}
