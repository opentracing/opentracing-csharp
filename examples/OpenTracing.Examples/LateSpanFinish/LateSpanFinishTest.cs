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

namespace OpenTracing.Examples.LateSpanFinish
{
    public class LateSpanFinishTest
    {
        private readonly MockTracer tracer = new MockTracer();

        [Fact]
        public void test()
        {
            // Create a Span manually and use it as parent of a pair of subtasks
            ISpan parentSpan = tracer.BuildSpan("parent").Start();
            using (IScope scope = tracer.ScopeManager.Activate(parentSpan, false))
            {
                SubmitTasks();
            }

            WaitForSpanCount(tracer, 2, DefaultTimeout);

            // Late-finish the parent Span now
            parentSpan.Finish();

            var spans = tracer.FinishedSpans();
            Assert.Equal(3, spans.Count);
            Assert.Equal("task1", spans[0].OperationName);
            Assert.Equal("task2", spans[1].OperationName);
            Assert.Equal("parent", spans[2].OperationName);

            TestUtils.AssertSameTrace(spans);

            Assert.Null(tracer.ActiveSpan);
        }

        // Fire away a few subtasks, passing a parent ISpan whose lifetime
        // is not tied at-all to the children.
        // NOTE: As opposed to Java, there is not need to reactivate the parent Span,
        // as the context is propagated by AsyncLocalScopeManager.
        private void SubmitTasks()
        {
            Task.Run(async () =>
            {
                using (IScope childScope1 = tracer.BuildSpan("task1").StartActive(true))
                {
                    await Task.Delay(55);
                }
            });

            Task.Run(async () =>
            {
                using (IScope childScope2 = tracer.BuildSpan("task2").StartActive(true))
                {
                    await Task.Delay(85);
                }
            });
        }
    }
}
