using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OpenTracing.Mock;
using Xunit;

using static OpenTracing.Examples.TestUtils;

namespace OpenTracing.Examples.MultipleCallbacks
{
    public class MultipleCallbacksTest
    {
        private readonly MockTracer _tracer = new MockTracer();

        [Fact]
        public void test()
        {
            Client client = new Client(_tracer);
            var tasks = new Task[3];

            var span = _tracer.BuildSpan("parent").Start();
            using (IScope scope = _tracer.ScopeManager.Activate(span, finishSpanOnDispose:false))
            {
                var rand = new Random();
                for (int i = 0; i < tasks.Length; i++)
                    tasks[i] = client.Send("task" + i, rand.Next(300));

                Task.WhenAll(tasks).ContinueWith(arg => span.Finish());
            }

            WaitForSpanCount(_tracer, 4, DefaultTimeout);

            List<MockSpan> spans = _tracer.FinishedSpans();
            Assert.Equal(4, spans.Count);
            Assert.Equal("parent", spans[3].OperationName);

            AssertSameTrace(spans);

            Assert.Null(_tracer.ActiveSpan);
        }
    }
}
