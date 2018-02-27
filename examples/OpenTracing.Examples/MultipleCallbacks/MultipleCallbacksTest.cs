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

namespace OpenTracing.Examples.MultipleCallbacks
{
    public class MultipleCallbacksTest
    {
        private readonly MockTracer tracer = new MockTracer();

        [Fact]
        public void test()
        {
            Client client = new Client(tracer);
            var tasks = new Task[3];

            var span = tracer.BuildSpan("parent").Start();
            using (IScope scope = tracer.ScopeManager.Activate(span, false))
            {
                var rand = new Random();
                for (int i = 0; i < tasks.Length; i++)
                    tasks[i] = client.Send("task" + i, rand.Next(300));

                Task.WhenAll(tasks).ContinueWith(arg => span.Finish());
            }

            WaitForSpanCount(tracer, 4, DefaultTimeout);

            List<MockSpan> spans = tracer.FinishedSpans();
            Assert.Equal(4, spans.Count);
            Assert.Equal("parent", spans[3].OperationName);

            AssertSameTrace(spans);

            Assert.Null(tracer.ActiveSpan);
        }
    }
}