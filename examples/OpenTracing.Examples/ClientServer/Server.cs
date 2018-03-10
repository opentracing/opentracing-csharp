using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading;
using OpenTracing.Propagation;
using OpenTracing.Tag;

namespace OpenTracing.Examples.ClientServer
{
    public class Server
    {
        private readonly BlockingCollection<Message> _queue;
        private readonly ITracer _tracer;

        public Server(BlockingCollection<Message> queue, ITracer tracer)
        {
            _queue = queue;
            _tracer = tracer;
        }

        private void Process(Message message)
        {
            ISpanContext context = _tracer.Extract(BuiltinFormats.TextMap, new TextMapExtractAdapter(message));
            using (IScope scope = _tracer.BuildSpan("receive")
                  .WithTag(Tags.SpanKind.Key, Tags.SpanKindServer)
                  .WithTag(Tags.Component.Key, "example-server")
                  .AsChildOf(context)
                  .StartActive(finishSpanOnDispose:true))
            {
            }
        }

        public void Start()
        {
            var thread = new Thread(() =>
            {
                // Wait for messages indefinitely, till
                // the queue has been marked as adding-complete.
                while (_queue.TryTake(out Message message, -1)) {
                    Process(message);
                }
            });
            thread.IsBackground = true;
            thread.Start();
        }

        public void Stop()
        {
            _queue.CompleteAdding();
        }
    }
}
