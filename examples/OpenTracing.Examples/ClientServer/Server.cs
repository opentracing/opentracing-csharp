/*
 * Copyright 2016-2018 The OpenTracing Authors
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except
 * in compliance with the License. You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software distributed under the License
 * is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
 * or implied. See the License for the specific language governing permissions and limitations under
 * the License.
 */
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