using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using OpenTracing.Propagation;
using OpenTracing.Tag;

namespace OpenTracing.Examples.ClientServer
{
    public class Client
    {
        private readonly BlockingCollection<Message> queue;
        private readonly ITracer tracer;

        public Client(BlockingCollection<Message> queue, ITracer tracer)
        {
            this.queue = queue;
            this.tracer = tracer;
        }

        public void Send()
        {
            var message = new Message();

            using (IScope scope = tracer.BuildSpan("send")
                    .WithTag(Tags.SpanKind.Key, Tags.SpanKindClient)
                    .WithTag(Tags.Component.Key, "example-client")
                    .StartActive(true))
            {
                tracer.Inject(scope.Span.Context, BuiltinFormats.TextMap, new TextMapInjectAdapter(message));
                queue.Add(message);
            }
        }
    }
}