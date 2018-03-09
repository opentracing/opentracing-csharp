using System;
using System.Collections.Generic;
using OpenTracing.Mock;
using OpenTracing.Tag;
using Xunit;

using static OpenTracing.Examples.TestUtils;

namespace OpenTracing.Examples.ListenerPerRequest
{
    // Each request has own instance of ResponseListener
    public class ListenerTest
    {
        private readonly MockTracer _tracer = new MockTracer();

        [Fact]
        public void test()
        {
            var client = new Client(_tracer);

            var responseTask = client.Send("message");
            responseTask.Wait(DefaultTimeout);
            string response = responseTask.Result;
            Assert.Equal("message:response", response);

            var finished = _tracer.FinishedSpans();
            Assert.Single(finished);
            Assert.NotNull(GetOneByTag(finished, Tags.SpanKind, Tags.SpanKindClient));

            Assert.Null(_tracer.ScopeManager.Active);
        }
    }
}
