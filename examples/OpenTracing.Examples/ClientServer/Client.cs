using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using OpenTracing.Propagation;
using OpenTracing.Tag;

namespace OpenTracing.Examples.ClientServer
{
    public class Client
    {
        private readonly BlockingCollection<Message> _queue;
        private readonly ITracer _tracer;

        public Client(BlockingCollection<Message> queue, ITracer tracer)
        {
            _queue = queue;
            _tracer = tracer;
        }

        public void Send()
        {
            var message = new Message();

            using (IScope scope = _tracer.BuildSpan("send")
                    .WithTag(Tags.SpanKind.Key, Tags.SpanKindClient)
                    .WithTag(Tags.Component.Key, "example-client")
                    .StartActive(finishSpanOnDispose:true))
            {
                _tracer.Inject(scope.Span.Context, BuiltinFormats.TextMap, new TextMapInjectAdapter(message));
                _queue.Add(message);
            }
        }
    }
}
