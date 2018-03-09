using System;
using System.Threading.Tasks;
using OpenTracing;

namespace OpenTracing.Examples.MultipleCallbacks
{
    public class Client
    {
        private readonly ITracer _tracer;

        public Client(ITracer tracer)
        {
            _tracer = tracer;
        }

        public async Task<string> Send<T>(T message, long milliseconds)
        {
            using (IScope scope = _tracer.BuildSpan("subtask").StartActive(finishSpanOnDispose:true))
            {
                await Task.Delay(TimeSpan.FromMilliseconds(milliseconds));
            }

            return message + "::response";
        }
    }
}
