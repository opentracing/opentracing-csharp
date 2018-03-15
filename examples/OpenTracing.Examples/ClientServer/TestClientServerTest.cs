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
        private readonly MockTracer _tracer = new MockTracer();
        private readonly BlockingCollection<Message> _queue = new BlockingCollection<Message>(10);
        private Server _server;

        public TestClientServerTest()
        {
            _server = new Server(_queue, _tracer);
            _server.Start();
        }

        void IDisposable.Dispose()
        {
            _server.Stop();
        }

        [Fact]
        public void test()
        {
            Client client = new Client(_queue, _tracer);
            client.Send();

            WaitForSpanCount(_tracer, 2, DefaultTimeout);

            var finished = _tracer.FinishedSpans();
            Assert.Equal(2, finished.Count);
            Assert.Equal(finished[0].Context.TraceId, finished[1].Context.TraceId);
            Assert.NotNull(GetOneByTag(finished, Tags.SpanKind, Tags.SpanKindClient));
            Assert.NotNull(GetOneByTag(finished, Tags.SpanKind, Tags.SpanKindServer));

            Assert.Null(_tracer.ScopeManager.Active);
        }
    }
}
