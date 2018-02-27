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