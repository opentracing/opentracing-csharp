using System;
using System.Linq;
using OpenTracing.Mock;
using Xunit;

namespace OpenTracing.Tests.Mock
{
    public class MockSpanTests
    {
        [Fact]
        public void SetOperationNameAfterFinishShouldThrow()
        {
            var tracer = new MockTracer();
            var span = tracer.BuildSpan("foo").Start();
            span.Finish();

            Assert.Throws<InvalidOperationException>(() => span.SetOperationName("bar"));
            Assert.Single(tracer.FinishedSpans()[0].GeneratedErrors);
        }

        [Fact]
        public void SetTagAFterFinishShouldThrow()
        {
            var tracer = new MockTracer();
            var span = tracer.BuildSpan("foo").Start();
            span.Finish();

            Assert.Throws<InvalidOperationException>(() => span.SetTag("foo", "bar"));
            Assert.Single(tracer.FinishedSpans()[0].GeneratedErrors);
        }

        [Fact]
        public void AddLogAFterFinishShouldThrow()
        {
            var tracer = new MockTracer();
            var span = tracer.BuildSpan("foo").Start();
            span.Finish();

            Assert.Throws<InvalidOperationException>(() => span.Log("bar"));
            Assert.Single(tracer.FinishedSpans()[0].GeneratedErrors);
        }

        [Fact]
        public void AddBaggageAFterFinishShouldThrow()
        {
            var tracer = new MockTracer();
            var span = tracer.BuildSpan("foo").Start();
            span.Finish();

            Assert.Throws<InvalidOperationException>(() => span.SetBaggageItem("foo", "bar"));
            Assert.Single(tracer.FinishedSpans()[0].GeneratedErrors);
        }
    }
}
