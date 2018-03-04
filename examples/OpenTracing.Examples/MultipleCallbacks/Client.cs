using System;
using System.Threading.Tasks;
using OpenTracing;

namespace OpenTracing.Examples.MultipleCallbacks
{
    public class Client
    {
        private readonly ITracer tracer;

        public Client(ITracer tracer)
        {
            this.tracer = tracer;
        }

        public async Task<String> Send<T>(T message, long milliseconds)
        {
            using (IScope scope = tracer.BuildSpan("subtask").StartActive(true))
            {
                await Task.Delay(TimeSpan.FromMilliseconds(milliseconds));
            }

            return message + "::response";
        }
    }
}