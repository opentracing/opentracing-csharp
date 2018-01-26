using System;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace OpenTracing.MockTracer.Tests
{
    public class MockSpanSpecs
    {
        [Fact]
        public void SetOperationNameAfterFinishShouldThrow()
        {
            var tracer = new OpenTracing.MockTracer.MockTracer();
            var span = tracer.BuildSpan("foo").Start();
            span.Finish();

            Action act = () => span.SetOperationName("bar");

            act.ShouldThrow<InvalidOperationException>();

            tracer.FinishedSpans.First().Errors.Count.Should().Be(1);
        }

        [Fact]
        public void SetTagAFterFinishShouldThrow()
        {
            var tracer = new MockTracer();
            var span = tracer.BuildSpan("foo").Start();
            span.Finish();

            Action act = () => span.SetTag("foo", "bar");

            act.ShouldThrow<InvalidOperationException>();

            tracer.FinishedSpans.First().Errors.Count.Should().Be(1);
        }

        [Fact]
        public void AddLogAFterFinishShouldThrow()
        {
            var tracer = new MockTracer();
            var span = tracer.BuildSpan("foo").Start();
            span.Finish();

            Action act = () => span.Log("test");

            act.ShouldThrow<InvalidOperationException>();

            tracer.FinishedSpans.First().Errors.Count.Should().Be(1);
        }

        [Fact]
        public void AddBaggageAFterFinishShouldThrow()
        {
            var tracer = new MockTracer();
            var span = tracer.BuildSpan("foo").Start();
            span.Finish();

            Action act = () => span.SetBaggageItem("foo", "bar");

            act.ShouldThrow<InvalidOperationException>();

            tracer.FinishedSpans.First().Errors.Count.Should().Be(1);
        }
    }
}