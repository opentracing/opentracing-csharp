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
using OpenTracing.Mock;
using OpenTracing.Tag;
using Xunit;

using static OpenTracing.Examples.TestUtils;

namespace OpenTracing.Examples.ListenerPerRequest
{
    // Each request has own instance of ResponseListener
    public class ListenerTest
    {
        private readonly MockTracer tracer = new MockTracer();

        [Fact]
        public void test()
        {
            var client = new Client(tracer);

            var responseTask = client.Send("message");
            responseTask.Wait(DefaultTimeout);
            String response = responseTask.Result;
            Assert.Equal("message:response", response);

            var finished = tracer.FinishedSpans();
            Assert.Single(finished);
            Assert.NotNull(GetOneByTag(finished, Tags.SpanKind, Tags.SpanKindClient));

            Assert.Null(tracer.ScopeManager.Active);
        }
    }
}