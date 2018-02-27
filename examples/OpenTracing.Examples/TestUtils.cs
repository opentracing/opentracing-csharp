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
using System.Threading;
using OpenTracing.Mock;
using OpenTracing.Tag;
using Xunit;

namespace OpenTracing.Examples
{
    public static class TestUtils
    {
        public static TimeSpan DefaultTimeout { get; } = TimeSpan.FromSeconds(5);

        public static void WaitForSpanCount (MockTracer tracer, int spanCount, TimeSpan timeout)
        {
            DateTime expiration = DateTime.Now + timeout;
            while (DateTime.Now < expiration && tracer.FinishedSpans().Count < spanCount) {
                Thread.Sleep(100);
            }
        }

        public static List<MockSpan> GetByTag<T>(List<MockSpan> spans, AbstractTag<T> key, T value)
        {
            var found = new List<MockSpan>(spans.Count);
            foreach (var span in spans)
            {
                if (span.Tags[key.Key].Equals(value))
                {
                    found.Add(span);
                }
            }
            return found;
        }

        public static MockSpan GetOneByTag<T>(List<MockSpan> spans, AbstractTag<T> key, T value)
        {
            var found = GetByTag(spans, key, value);
            if (found.Count > 1)
            {
                throw new ArgumentException("there is more than one span with tag '"
                        + key.Key + "' and value '" + value + "'");
            }
            if (found.Count == 0)
            {
                return null;
            }
            else
            {
                return found[0];
            }
        }

        public static void SortByStartTimestamp(List<MockSpan> spans)
        {
            spans.Sort((span1, span2) => span1.StartTimestamp.CompareTo(span2.StartTimestamp));
        }


        public static void AssertSameTrace(List<MockSpan> spans)
        {
            for (int i = 0; i < spans.Count - 1; i++)
            {
                Assert.True(spans[spans.Count - 1].FinishTimestamp >= spans[i].FinishTimestamp);
                Assert.Equal(spans[spans.Count - 1].Context.TraceId, spans[i].Context.TraceId);
                Assert.Equal(spans[spans.Count - 1].Context.SpanId, spans[i].ParentId);
            }
        }
    }
}