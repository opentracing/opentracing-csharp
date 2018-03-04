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
        private readonly BlockingCollection<Message> queue;
        private readonly ITracer tracer;

        public Server(BlockingCollection<Message> queue, ITracer tracer)
        {
            this.queue = queue;
            this.tracer = tracer;
        }

        private void Process(Message message)
        {
            ISpanContext context = tracer.Extract(BuiltinFormats.TextMap, new TextMapExtractAdapter(message));
            using (IScope scope = tracer.BuildSpan("receive")
                  .WithTag(Tags.SpanKind.Key, Tags.SpanKindServer)
                  .WithTag(Tags.Component.Key, "example-server")
                  .AsChildOf(context)
                  .StartActive(true))
            {
            }
        }

        public void Start()
        {
            var thread = new Thread(() =>
            {
                // Wait for messages indefinitely, till
                // the queue has been marked as adding-complete.
                Message message;
                while (queue.TryTake(out message, -1)) {
                    Process(message);
                }
            });
            thread.IsBackground = true;
            thread.Start();
        }

        public void Stop()
        {
            queue.CompleteAdding();
        }
    }
}