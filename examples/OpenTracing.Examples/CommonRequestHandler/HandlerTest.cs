using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OpenTracing.Mock;
using OpenTracing.Tag;
using Xunit;

using static OpenTracing.Examples.TestUtils;

// There is only one instance of 'RequestHandler' per 'Client'. Methods of 'RequestHandler' are
// executed concurrently in different threads which are reused (common pool). Therefore we cannot
// use current active span and Activate. So one issue here is setting correct parent span.
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
        public void TwoRequests()
        {
            var responseTask = _client.Send("message");
            var responseTask2 = _client.Send("message2");

            responseTask.Wait(DefaultTimeout);
            responseTask2.Wait(DefaultTimeout);
            Assert.Equal("message:response", responseTask.Result);
            Assert.Equal("message2:response", responseTask2.Result);

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
        [Fact]
        public void ParentNotPickedUp()
        {
            using (IScope scope = _tracer.BuildSpan("parent").StartActive(finishSpanOnDispose:true))
            {
                var responseTask = _client.Send("no_parent");
                responseTask.Wait(DefaultTimeout);
                string response = responseTask.Result;
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

        // Solution is bad because parent is per client (we don't have better choice).
        // Therefore all client requests will have the same parent.
        // But if client is long living and injected/reused in different places then initial parent will not be correct.
        [Fact]
        public void BadSolutionToSetParent()
        {
            Client testClient;
            using (IScope scope = _tracer.BuildSpan("parent").StartActive(finishSpanOnDispose:true))
            {
                testClient = new Client(new RequestHandler(_tracer, scope.Span.Context));

                var responseTask = testClient.Send("correct_parent");
                responseTask.Wait(DefaultTimeout);
                string response = responseTask.Result;
                Assert.Equal("correct_parent:response", response);
            }

            // Send second request, now there is no active parent, but it will be set, ups
            var responseTask2 = testClient.Send("wrong_parent");
            responseTask2.Wait(DefaultTimeout);
            string response2 = responseTask2.Result;
            Assert.Equal("wrong_parent:response", response2);

            var finished = _tracer.FinishedSpans();
            Assert.Equal(3, finished.Count);

            SortByStartTimestamp(finished);

            MockSpan parent = GetOneByOperationName(finished, "parent");
            Assert.NotNull(parent);

            // now there is parent/child relation between first and second span:
            Assert.Equal(parent.Context.SpanId, finished[1].ParentId);

            // third span should not have parent, but it has, damn it
            Assert.Equal(parent.Context.SpanId, finished[2].ParentId);
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