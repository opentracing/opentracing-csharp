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
        public async Task test()
        {
            Client client = new Client(_tracer);
            var tasks = new Task[3];

            var span = _tracer.BuildSpan("parent").Start();
            using (IScope scope = _tracer.ScopeManager.Activate(span, finishSpanOnDispose:true))
            {
                var rand = new Random();
                for (int i = 0; i < tasks.Length; i++)
                    tasks[i] = client.Send("task" + i, rand.Next(300));

                await Task.WhenAll(tasks);
            }

            List<MockSpan> spans = _tracer.FinishedSpans();
            Assert.Equal(4, spans.Count);
            Assert.Equal("parent", spans[3].OperationName);

            AssertSameTrace(spans);

            Assert.Null(_tracer.ActiveSpan);
        }
    }
}
