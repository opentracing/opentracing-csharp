using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using OpenTracing.Propagation;
using Xunit;

namespace OpenTracing.MockTracer.Tests
{
    public class MockTracerSpecs
    {
        [Fact(DisplayName = "Root span's fields should all be set correctly")]
        public void RootSpanShouldHavePropertiesSetCorrectly()
        {
            var tracer = new MockTracer();
            var span = tracer.BuildSpan("foo").WithTag("actorId", 1)
                .WithTag("actorPath", "/user/foo/test")
                .WithTag("singleton", false)
                .WithStartTimestamp(DateTimeOffset.UtcNow.AddMilliseconds(-10)).Start();

            var timeStamp = DateTimeOffset.UtcNow.AddMilliseconds(-8);
            span.Log(timeStamp, "foo");
            span.Log(timeStamp.AddMilliseconds(1), new[] { new KeyValuePair<string, object>("foo1", true) });
            span.SetBaggageItem("baggageIsDifferentThanTags", "apparently");

            span.Finish();

            var testSpan = (MockSpan)span;

            var recordedSpans = tracer.FinishedSpans;
            recordedSpans.First().Should().Be(testSpan);

            testSpan.IsFinished.Should().BeTrue();
            testSpan.References.Should().BeEmpty("We did not set any reference");
            testSpan.Context.As<MockSpan.MockContext>().SpanId.Should()
                .NotBe(testSpan.Context.As<MockSpan.MockContext>().TraceId, "traceId and spanId should be set independently");
            testSpan.Errors.Should().BeEmpty();
            testSpan.FinishTime.Should().BeAfter(testSpan.StartTime);
            testSpan.Tags.Keys.Should().BeEquivalentTo("actorId", "actorPath", "singleton");
            testSpan.Tags.Values.Should().BeEquivalentTo(1, "/user/foo/test", false);
            testSpan.Logs.Count.Should().Be(2);

            var log1 = testSpan.Logs[0];
            log1.Fields.Count.Should().Be(1);
            log1.Fields["event"].Should().Be("foo");
            log1.TimeStamp.Should().Be(timeStamp);

            var log2 = testSpan.Logs[1];
            log2.Fields.Count.Should().Be(1);
            log2.Fields["foo1"].Should().Be(true);
            log2.TimeStamp.Should().Be(timeStamp.AddMilliseconds(1));

            var baggage = testSpan.Context.GetBaggageItems().ToList();
            baggage.Count.Should().Be(1);
            testSpan.GetBaggageItem("baggageIsDifferentThanTags").Should().Be("apparently");
        }

        [Fact(DisplayName = "Child span's fields should be set correctly")]
        public void ChildSpanPropertiesShouldBeSetCorrectly()
        {
            // create and finish a root span
            var tracer = new MockTracer();
            using (var parent = tracer.BuildSpan("parent").WithStartTimestamp(DateTimeOffset.UtcNow).Start())
            {
                var child = tracer.BuildSpan("child").WithStartTimestamp(DateTimeOffset.UtcNow.AddMilliseconds(100))
                    .AsChildOf(parent).Start();

                child.Finish(DateTimeOffset.UtcNow.AddMilliseconds(900));
                parent.Finish(DateTimeOffset.UtcNow.AddMilliseconds(1000));
            }

            var finishedSpans = tracer.FinishedSpans.ToList();

          
            finishedSpans.Count.Should().Be(2);
            var c = finishedSpans[0];
            var p = finishedSpans[1];
            c.OperationName.Should().Be("child");
            p.OperationName.Should().Be("parent");
            p.SpanId.Should().Be(c.ParentId);
            p.TraceId.Should().Be(c.TraceId);
            c.SpanId.Should().BeGreaterThan(p.SpanId);
        }

        [Fact(DisplayName = "Spans should use explicit timestamps when specified")]
        public void ExplicitTimeStampShouldBeUsed()
        {
            // create and finish a root span
            var tracer = new MockTracer();
            var startTime = DateTimeOffset.UtcNow.AddMilliseconds(-100);
            tracer.BuildSpan("foo").WithStartTimestamp(startTime).Start().Finish();

            var finishedSpan = tracer.FinishedSpans.First();
            finishedSpan.StartTime.Equals(startTime);
        }

        [Fact(DisplayName = "TextMapPropagator should work as expected with TextMap format")]
        public void TextMapPropagatorShouldWorkWithTextMap()
        {
            var tracer = new MockTracer(MockTracer.TextMapPropagator);
            var injectMap = new Dictionary<string, string> {["foobag"] = "donttouch"};

            var parentSpan = tracer.BuildSpan("foo").Start();
            parentSpan.SetBaggageItem("foobag", "fooitem");
            parentSpan.Finish();

            tracer.Inject(parentSpan.Context, Formats.TextMap, new DictionaryCarrier(injectMap));

            var extract = tracer.Extract(Formats.TextMap, new DictionaryCarrier(injectMap));

            var childSpan = tracer.BuildSpan("bar")
                .AsChildOf(extract).Start();
            childSpan.SetBaggageItem("barbag", "baritem");
            childSpan.Finish();

            var finishedSpans = tracer.FinishedSpans.ToList();

            finishedSpans.Count.Should().Be(2);
            finishedSpans[0].TraceId.Should().Be(finishedSpans[1].TraceId);
            finishedSpans[0].SpanId.Should().Be(finishedSpans[1].ParentId);
            finishedSpans[0].SpanId.Should().NotBe(finishedSpans[1].SpanId);
            finishedSpans[0].GetBaggageItem("foobag").Should().Be("fooitem");
            finishedSpans[0].GetBaggageItem("barbag").Should().BeNullOrEmpty();
            finishedSpans[1].GetBaggageItem("foobag").Should().Be("fooitem");
            finishedSpans[1].GetBaggageItem("barbag").Should().Be("baritem");
            injectMap["foobag"].Should().Be("donttouch");
        }

        [Fact(DisplayName = "TextMapPropagator should work as expected with HTTP Headers format")]
        public void TextMapPropagatorShouldWorkWithHttpHeaders()
        {
            var tracer = new MockTracer(MockTracer.TextMapPropagator);
            var injectMap = new Dictionary<string, string>();

            var parentSpan = tracer.BuildSpan("foo").Start();
            parentSpan.Finish();

            tracer.Inject(parentSpan.Context, Formats.HttpHeaders, new DictionaryCarrier(injectMap));

            var extract = tracer.Extract(Formats.HttpHeaders, new DictionaryCarrier(injectMap));

            tracer.BuildSpan("bar")
                .AsChildOf(extract).Start().Finish();

            var finishedSpans = tracer.FinishedSpans.ToList();

            finishedSpans.Count.Should().Be(2);
            finishedSpans[0].TraceId.Should().Be(finishedSpans[1].TraceId);
            finishedSpans[0].SpanId.Should().Be(finishedSpans[1].ParentId);
            finishedSpans[0].SpanId.Should().NotBe(finishedSpans[1].SpanId);
        }

        [Fact(DisplayName = "MockTracer.Reset should clear all finished spans from memory.")]
        public void MockTracerResetShouldClearFinishedSpans()
        {
            var tracer = new MockTracer();

            tracer.BuildSpan("foo").Start().Finish();

            tracer.FinishedSpans.Count().Should().Be(1);
            tracer.Reset();
            tracer.FinishedSpans.Count().Should().Be(0);
        }

        [Fact(DisplayName = "FollowFrom references should be set correctly")]
        public void FollowFromReferenceSpec()
        {
            var tracer = new MockTracer(MockTracer.TextMapPropagator);

            var precedent = (MockSpan)tracer.BuildSpan("precedent").Start();
            var following = (MockSpan)tracer.BuildSpan("follows")
                .AddReference(References.FollowsFrom, precedent.Context)
                .Start();

            precedent.SpanId.Should().Be(following.ParentId); // should still be parent ID, since there's no explicit childof
            following.References.Count.Should().Be(1);

            var followingRef = following.References[0];

            new MockSpan.Reference((MockSpan.MockContext)precedent.Context, References.FollowsFrom).Should().Be(followingRef);
        }

        [Fact(DisplayName = "Spans with multiple references should be set correctly")]
        public void MultiReferenceSpec()
        {
            var tracer = new MockTracer(MockTracer.TextMapPropagator);
            var parent = (MockSpan)tracer.BuildSpan("parent").Start();
            var precedent = (MockSpan)tracer.BuildSpan("precedent").Start();

            var followingSpan = (MockSpan)tracer.BuildSpan("follows")
                .AddReference(References.FollowsFrom, precedent.Context)
                .AsChildOf(parent.Context)
                .Start();

            parent.SpanId.Should().Be(followingSpan.ParentId);
            followingSpan.References.Count.Should().Be(2);

            var followsFromRef = followingSpan.References[0];
            var parentRef = followingSpan.References[1];

            new MockSpan.Reference((MockSpan.MockContext)precedent.Context, References.FollowsFrom).Should()
                .Be(followsFromRef);
            new MockSpan.Reference((MockSpan.MockContext)parent.Context, References.ChildOf).Should()
                .Be(parentRef);
        }

        [Fact(DisplayName = "Spans with multiple references should merge baggage correctly")]
        public void MultiReferencesBaggageSpec()
        {
            var tracer = new MockTracer(MockTracer.TextMapPropagator);
            var parent = (MockSpan)tracer.BuildSpan("parent").Start();
            parent.SetBaggageItem("parent", "foo");
            var precedent = (MockSpan)tracer.BuildSpan("precedent").Start();
            precedent.SetBaggageItem("precedent", "bar");

            var followingSpan = (MockSpan)tracer.BuildSpan("follows")
                .AddReference(References.FollowsFrom, precedent.Context)
                .AsChildOf(parent.Context)
                .Start();

            followingSpan.GetBaggageItem("parent").Should().Be("foo");
            followingSpan.GetBaggageItem("precedent").Should().Be("bar");
        }

        [Fact(DisplayName = "Spans with non-standard references should flow correctly")]
        public void NonStandardReferenceSpec()
        {
            var tracer = new MockTracer(MockTracer.TextMapPropagator);
            var parent = (MockSpan)tracer.BuildSpan("parent").Start();

            var nextSpan = (MockSpan)tracer.BuildSpan("follows").AddReference("a_reference", parent.Context)
                .Start();

            parent.SpanId.Should().Be(nextSpan.ParentId);
            nextSpan.References.Count.Should().Be(1);
            nextSpan.References[0].Should().Be(new MockSpan.Reference((MockSpan.MockContext)parent.Context, "a_reference"));
        }

        [Fact(DisplayName = "SpanBuilder.ChildOf with null parent should not throw")]
        public void ChildOfWithNullParentShouldNotThrow()
        {
            var tracer = new MockTracer();
            ISpan parent = null;

            var span = tracer.BuildSpan("foo").AsChildOf(parent).Start();
            span.Finish();
        }
    }
}
