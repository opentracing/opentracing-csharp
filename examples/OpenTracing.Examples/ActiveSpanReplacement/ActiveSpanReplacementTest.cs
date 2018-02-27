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
using System.Threading.Tasks;
using OpenTracing.Mock;
using Xunit;

using static OpenTracing.Examples.TestUtils;

namespace OpenTracing.Examples.ActiveSpanReplacement
{
    public class ActiveSpanReplacementTest
    {
        private readonly MockTracer tracer = new MockTracer();

        [Fact]
        public void test()
        {
            // Start an isolated task and query for its result in another task/thread
            ISpan initialSpan = tracer.BuildSpan("initial").Start();

            // Explicitly pass a Span to be finished once a late calculation is done.
            SubmitAnotherTask(initialSpan);

            WaitForSpanCount(tracer, 3, DefaultTimeout);

            var spans = tracer.FinishedSpans();
            Assert.Equal(3, spans.Count);
            Assert.Equal("initial", spans[0].OperationName); // Isolated task
            Assert.Equal("subtask", spans[1].OperationName);
            Assert.Equal("task", spans[2].OperationName);

            // task/subtask are part of the same trace, and subtask is a child of task
            Assert.Equal(spans[1].Context.TraceId, spans[2].Context.TraceId);
            Assert.Equal(spans[2].Context.SpanId, spans[1].ParentId);

            // initial task is not related in any way to those two tasks
            Assert.NotEqual(spans[0].Context.TraceId, spans[1].Context.TraceId);
            Assert.Equal(0, spans[0].ParentId);

            Assert.Null(tracer.ScopeManager.Active);
        }

        private void SubmitAnotherTask(ISpan initialSpan)
        {
            Task.Run(async () =>
            {
                // Create a new Span for this task
                using (IScope taskScope = tracer.BuildSpan("task").StartActive(true))
                {
                    // Simulate work strictly related to the initial Span
                    // and finish it.
                    using (IScope initialScope = tracer.ScopeManager.Activate(initialSpan, true))
                    {
                        await Task.Delay(50);
                    }

                    // Restore the span for this task and create a subspan
                    using (IScope subTaskScope = tracer.BuildSpan("subtask").StartActive(true))
                    {
                    }
                }
            });
        }
    }
}