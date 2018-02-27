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
using OpenTracing.Mock;
using OpenTracing.Tag;
using Xunit;

using static OpenTracing.Examples.TestUtils;

namespace OpenTracing.Examples.ClientServer
{
    public class TestClientServerTest : IDisposable
    {
        private readonly MockTracer tracer = new MockTracer();
        private readonly BlockingCollection<Message> queue = new BlockingCollection<Message>(10);
        private Server server;

        public TestClientServerTest()
        {
            server = new Server(queue, tracer);
            server.Start();
        }

        void IDisposable.Dispose()
        {
            server.Stop();
        }

        [Fact]
        public void test()
        {
            Client client = new Client(queue, tracer);
            client.Send();

            WaitForSpanCount(tracer, 2, DefaultTimeout);

            var finished = tracer.FinishedSpans();
            Assert.Equal(2, finished.Count);
            Assert.Equal(finished[0].Context.TraceId, finished[1].Context.TraceId);
            Assert.NotNull(GetOneByTag(finished, Tags.SpanKind, Tags.SpanKindClient));
            Assert.NotNull(GetOneByTag(finished, Tags.SpanKind, Tags.SpanKindServer));

            Assert.Null(tracer.ScopeManager.Active);
        }
    }
}
