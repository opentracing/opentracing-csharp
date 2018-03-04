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
