using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        public async Task test()
        {
            var client = new Client(_tracer);

            string response = await client.Send("message");
            Assert.Equal("message:response", response);

            var finished = _tracer.FinishedSpans();
            Assert.Single(finished);
            Assert.NotNull(GetOneByTag(finished, Tags.SpanKind, Tags.SpanKindClient));

            Assert.Null(_tracer.ScopeManager.Active);
        }
    }
}
