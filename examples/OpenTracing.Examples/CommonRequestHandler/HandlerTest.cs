using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OpenTracing.Mock;
using OpenTracing.Tag;
using Xunit;

using static OpenTracing.Examples.TestUtils;

// There is only one instance of 'RequestHandler' per 'Client'. Methods of 'RequestHandler' are
// executed concurrently in different threads which are reused (common pool). But as the active Span
// is properly propagated we can rely on getting/setting the active one.
namespace OpenTracing.Examples.CommonRequestHandler
{
    public class HandlerTest
    {
        private readonly MockTracer _tracer = new MockTracer();
        private readonly Client _client;

        public HandlerTest()
        {
            _client = new Client(new RequestHandler(_tracer));
        }

        [Fact]
        public async Task TwoRequests()
        {
            string response = await _client.Send("message");
            string response2 = await _client.Send("message2");

            Assert.Equal("message:response", response);
            Assert.Equal("message2:response", response2);

            var finished = _tracer.FinishedSpans();
            Assert.Equal(2, finished.Count);

            Assert.Equal(Tags.SpanKindClient, finished[0].Tags[Tags.SpanKind.Key]);
            Assert.Equal(Tags.SpanKindClient, finished[1].Tags[Tags.SpanKind.Key]);

            Assert.NotEqual(finished[0].Context.TraceId, finished[1].Context.TraceId);
            Assert.Equal(0, finished[0].ParentId);
            Assert.Equal(0, finished[1].ParentId);

            Assert.Null(_tracer.ScopeManager.Active);
        }

        // active parent is not picked up by child.
        // Need to explicitly tell to RequestHandler to ignore any active Span.
        [Fact]
        public async Task ParentNotPickedUp()
        {
            var testClient = new Client(new RequestHandler(_tracer, ignoreActiveSpan:true));
            using (IScope scope = _tracer.BuildSpan("parent").StartActive(finishSpanOnDispose:true))
            {
                string response = await testClient.Send("no_parent");
                Assert.Equal("no_parent:response", response);
            }

            var finished = _tracer.FinishedSpans();
            Assert.Equal(2, finished.Count);

            MockSpan child = GetOneByOperationName(finished, RequestHandler.OperationName);
            Assert.NotNull(child);

            MockSpan parent = GetOneByOperationName(finished, "parent");
            Assert.NotNull(parent);

            // Here check that there is no parent-child relation although it should be because child is
            // created when parent is active
            Assert.NotEqual(parent.Context.SpanId, child.ParentId);
        }

        [Fact]
        public async Task ParentPickedUp()
        {
            var testClient = new Client(new RequestHandler(_tracer));
            using (IScope scope = _tracer.BuildSpan("parent").StartActive(finishSpanOnDispose:true))
            {
                string response = await testClient.Send("correct_parent");
                Assert.Equal("correct_parent:response", response);
            }

            // Send second request, now there is no active parent.
            string response2 = await testClient.Send("no_parent");
            Assert.Equal("no_parent:response", response2);

            var finished = _tracer.FinishedSpans();
            Assert.Equal(3, finished.Count);

            SortByStartTimestamp(finished);

            MockSpan parent = GetOneByOperationName(finished, "parent");
            Assert.NotNull(parent);

            // now there is parent/child relation between first and second span:
            Assert.Equal(parent.Context.SpanId, finished[1].ParentId);

            // third span should not have parent.
            Assert.Equal(0, finished[2].ParentId);
        }

        private static MockSpan GetOneByOperationName(List<MockSpan> spans, string name)
        {
            MockSpan found = null;
            foreach (MockSpan span in spans)
            {
                if (name == span.OperationName)
                {
                    if (found != null)
                    {
                        throw new ArgumentException("there is more than one span with operation name '"
                                + name + "'");
                    }
                    found = span;
                }
            }
            return found;
        }
    }
}
