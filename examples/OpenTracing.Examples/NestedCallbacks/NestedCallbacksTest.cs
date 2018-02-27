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

namespace OpenTracing.Examples.NestedCallbacks
{
    public class NestedCallbacksTest
    {
        private readonly MockTracer tracer = new MockTracer();

        [Fact]
        public void test()
        {
            ISpan span = tracer.BuildSpan("one").Start();
            SubmitCallbacks(span);

            WaitForSpanCount(tracer, 1, DefaultTimeout);

            var spans = tracer.FinishedSpans();
            Assert.Single(spans);
            Assert.Equal("one", spans[0].OperationName);

            var tags = spans[0].Tags;
            Assert.Equal(3, tags.Count);
            for (int i = 1; i <= 3; i++)
            {
                Assert.Equal(i.ToString(), tags["key" + i]);
            }

            Assert.Null(tracer.ScopeManager.Active);
        }

        private void SubmitCallbacks(ISpan span)
        {
            // Manually activate span so it gets
            // propagated as the active one in the nested tasks.
            using (tracer.ScopeManager.Activate(span, false))
            {
                Task.Run(() =>
                {
                    tracer.ActiveSpan.SetTag("key1", "1");

                    Task.Run(() =>
                    {
                        tracer.ActiveSpan.SetTag("key2", "2");

                        Task.Run(() =>
                        {
                            tracer.ActiveSpan.SetTag("key3", "3");
                            tracer.ActiveSpan.Finish();
                        });
                    });
                });
            }
        }
    }
}