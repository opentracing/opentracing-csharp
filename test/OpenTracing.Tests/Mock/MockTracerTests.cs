using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OpenTracing.Mock;
using OpenTracing.Propagation;
using Xunit;

namespace OpenTracing.Tests.Mock
{
    public class MockTracerTests
    {
        private static readonly DateTimeOffset FixedStartTimestamp = GetTestTimestamp(0);
        private static readonly DateTimeOffset FixedFinishTimestamp = GetTestTimestamp(999);

        private static DateTimeOffset GetTestTimestamp(int millisecond)
        {
            return new DateTimeOffset(2000, 1, 1, 12, 0, 0, millisecond, TimeSpan.Zero);
        }

        [Fact]
        public void TestRootSpan()
        {
            // Create and finish a root span.
            var tracer = new MockTracer();

            var span = tracer.BuildSpan("tester")
                .WithStartTimestamp(FixedStartTimestamp)
                .Start();

            span.SetTag("string", "foo");
            span.SetTag("int", 7);
            span.Log("foo");
            var fields = new Dictionary<string, object> { { "f1", 4 }, { "f2", "two" } };
            span.Log(GetTestTimestamp(2), fields);
            span.Log(GetTestTimestamp(3), "event name");
            span.Finish(FixedFinishTimestamp);

            List<MockSpan> finishedSpans = tracer.FinishedSpans();

            // Check that the span looks right.

            Assert.Single(finishedSpans);
            MockSpan finishedSpan = finishedSpans[0];
            Assert.Equal("tester", finishedSpan.OperationName);
            Assert.Equal(0, finishedSpan.ParentId);
            Assert.NotEqual(0, finishedSpan.Context.TraceId);
            Assert.NotEqual(0, finishedSpan.Context.SpanId);
            Assert.Equal(FixedStartTimestamp, finishedSpan.StartTimestamp);
            Assert.Equal(FixedFinishTimestamp, finishedSpan.FinishTimestamp);

            var tags = finishedSpan.Tags;
            Assert.Equal(2, tags.Count);
            Assert.Equal(7, tags["int"]);
            Assert.Equal("foo", tags["string"]);

            var logs = finishedSpan.LogEntries;
            Assert.Equal(3, logs.Count);
            {
                MockSpan.LogEntry log = logs[0];
                Assert.Single(log.Fields);
                Assert.Equal("foo", log.Fields["event"]);
            }
            {
                MockSpan.LogEntry log = logs[1];
                Assert.Equal(GetTestTimestamp(2), log.Timestamp);
                Assert.Equal(4, log.Fields["f1"]);
                Assert.Equal("two", log.Fields["f2"]);
            }
            {
                MockSpan.LogEntry log = logs[2];
                Assert.Equal(GetTestTimestamp(3), log.Timestamp);
                Assert.Equal("event name", log.Fields["event"]);
            }
        }

        [Fact]
        public void TestChildSpan()
        {
            // Create and finish a root span.
            MockTracer tracer = new MockTracer();
            ISpan originalParent = tracer.BuildSpan("parent").WithStartTimestamp(GetTestTimestamp(100)).Start();
            ISpan originalChild = tracer.BuildSpan("child").WithStartTimestamp(GetTestTimestamp(200)).AsChildOf(originalParent).Start();
            originalChild.Finish(GetTestTimestamp(800));
            originalParent.Finish(GetTestTimestamp(900));

            List<MockSpan> finishedSpans = tracer.FinishedSpans();

            // Check that the spans look right.
            Assert.Equal(2, finishedSpans.Count);
            MockSpan child = finishedSpans[0];
            MockSpan parent = finishedSpans[1];
            Assert.Equal("child", child.OperationName);
            Assert.Equal("parent", parent.OperationName);
            Assert.Equal(parent.Context.SpanId, child.ParentId);
            Assert.Equal(parent.Context.TraceId, child.Context.TraceId);
        }

        [Fact]
        public void TestStartTimestamp()
        {
            MockTracer tracer = new MockTracer();
            DateTimeOffset startTimestamp;
            {
                ISpanBuilder fooSpan = tracer.BuildSpan("foo");
                Thread.Sleep(20);
                startTimestamp = DateTimeOffset.Now;
                fooSpan.Start().Finish();
            }
            List<MockSpan> finishedSpans = tracer.FinishedSpans();

            Assert.Single(finishedSpans);
            MockSpan span = finishedSpans[0];
            Assert.True(startTimestamp <= span.StartTimestamp);
            Assert.True(DateTimeOffset.Now >= span.FinishTimestamp);
        }

        [Fact]
        public void TestStartExplicitTimestamp()
        {
            MockTracer tracer = new MockTracer();
            DateTimeOffset startTimestamp = FixedStartTimestamp;
            {
                tracer.BuildSpan("foo")
                        .WithStartTimestamp(startTimestamp)
                        .Start()
                        .Finish();
            }
            List<MockSpan> finishedSpans = tracer.FinishedSpans();

            Assert.Single(finishedSpans);
            Assert.Equal(startTimestamp, finishedSpans[0].StartTimestamp);
        }

        [Fact]
        public void TestTextMapPropagatorTextMap()
        {
            MockTracer tracer = new MockTracer(Propagators.TextMap);
            var injectMap = new Dictionary<string, string> { { "foobag", "donttouch" } };
            {
                ISpan parentSpan = tracer.BuildSpan("foo")
                        .Start();
                parentSpan.SetBaggageItem("foobag", "fooitem");
                parentSpan.Finish();

                tracer.Inject(parentSpan.Context, BuiltinFormats.TextMap,
                        new TextMapInjectAdapter(injectMap));

                ISpanContext extract = tracer.Extract(BuiltinFormats.TextMap, new TextMapExtractAdapter(injectMap));

                ISpan childSpan = tracer.BuildSpan("bar")
                        .AsChildOf(extract)
                        .Start();
                childSpan.SetBaggageItem("barbag", "baritem");
                childSpan.Finish();
            }
            List<MockSpan> finishedSpans = tracer.FinishedSpans();

            Assert.Equal(2, finishedSpans.Count);
            Assert.Equal(finishedSpans[0].Context.TraceId, finishedSpans[1].Context.TraceId);
            Assert.Equal(finishedSpans[0].Context.SpanId, finishedSpans[1].ParentId);
            Assert.Equal("fooitem", finishedSpans[0].GetBaggageItem("foobag"));
            Assert.Null(finishedSpans[0].GetBaggageItem("barbag"));
            Assert.Equal("fooitem", finishedSpans[1].GetBaggageItem("foobag"));
            Assert.Equal("baritem", finishedSpans[1].GetBaggageItem("barbag"));
            Assert.Equal("donttouch", injectMap["foobag"]);
        }

        [Fact]
        public void TestTextMapPropagatorHttpHeaders()
        {
            MockTracer tracer = new MockTracer(Propagators.TextMap);
            {
                ISpan parentSpan = tracer.BuildSpan("foo")
                        .Start();
                parentSpan.Finish();

                var injectMap = new Dictionary<string, string>();
                tracer.Inject(parentSpan.Context, BuiltinFormats.HttpHeaders,
                        new TextMapInjectAdapter(injectMap));

                ISpanContext extract = tracer.Extract(BuiltinFormats.HttpHeaders, new TextMapExtractAdapter(injectMap));

                tracer.BuildSpan("bar")
                        .AsChildOf(extract)
                        .Start()
                        .Finish();
            }
            List<MockSpan> finishedSpans = tracer.FinishedSpans();

            Assert.Equal(2, finishedSpans.Count);
            Assert.Equal(finishedSpans[0].Context.TraceId, finishedSpans[1].Context.TraceId);
            Assert.Equal(finishedSpans[0].Context.SpanId, finishedSpans[1].ParentId);
        }

        [Fact]
        public void TestActiveSpan()
        {
            MockTracer mockTracer = new MockTracer();
            Assert.Null(mockTracer.ActiveSpan);

            using (mockTracer.BuildSpan("foo").StartActive(finishSpanOnDispose: true))
            {
                Assert.Equal(mockTracer.ScopeManager.Active.Span, mockTracer.ActiveSpan);
            }

            Assert.Null(mockTracer.ActiveSpan);
        }

        [Fact]
        public void TestReset()
        {
            MockTracer mockTracer = new MockTracer();

            mockTracer.BuildSpan("foo")
                .Start()
                .Finish();

            Assert.Single(mockTracer.FinishedSpans());
            mockTracer.Reset();
            Assert.Empty(mockTracer.FinishedSpans());
        }

        [Fact]
        public void TestFollowFromReference()
        {
            MockTracer tracer = new MockTracer(Propagators.TextMap);
            MockSpan precedent = (MockSpan)tracer.BuildSpan("precedent").Start();

            MockSpan followingSpan = (MockSpan)tracer.BuildSpan("follows")
                .AddReference(References.FollowsFrom, precedent.Context)
                .Start();

            Assert.Equal(precedent.Context.SpanId, followingSpan.ParentId);
            Assert.Single(followingSpan.References);

            MockSpan.Reference followsFromRef = followingSpan.References[0];

            Assert.Equal(new MockSpan.Reference(precedent.Context, References.FollowsFrom), followsFromRef);
        }

        [Fact]
        public void TestMultiReferences()
        {
            MockTracer tracer = new MockTracer(Propagators.TextMap);
            MockSpan parent = (MockSpan)tracer.BuildSpan("parent").Start();
            MockSpan precedent = (MockSpan)tracer.BuildSpan("precedent").Start();

            MockSpan followingSpan = (MockSpan)tracer.BuildSpan("follows")
                .AddReference(References.FollowsFrom, precedent.Context)
                .AsChildOf(parent.Context)
                .Start();

            Assert.Equal(parent.Context.SpanId, followingSpan.ParentId);
            Assert.Equal(2, followingSpan.References.Count);

            MockSpan.Reference followsFromRef = followingSpan.References[0];
            MockSpan.Reference parentRef = followingSpan.References[1];

            Assert.Equal(new MockSpan.Reference(precedent.Context, References.FollowsFrom), followsFromRef);
            Assert.Equal(new MockSpan.Reference(parent.Context, References.ChildOf), parentRef);
        }

        [Fact]
        public void TestMultiReferencesBaggage()
        {
            MockTracer tracer = new MockTracer(Propagators.TextMap);
            MockSpan parent = (MockSpan)tracer.BuildSpan("parent").Start();
            parent.SetBaggageItem("parent", "foo");
            MockSpan precedent = (MockSpan)tracer.BuildSpan("precedent").Start();
            precedent.SetBaggageItem("precedent", "bar");

            MockSpan followingSpan = (MockSpan)tracer.BuildSpan("follows")
                .AddReference(References.FollowsFrom, precedent.Context)
                .AsChildOf(parent.Context)
                .Start();

            Assert.Equal("foo", followingSpan.GetBaggageItem("parent"));
            Assert.Equal("bar", followingSpan.GetBaggageItem("precedent"));
        }

        [Fact]
        public void TestNonStandardReference()
        {
            MockTracer tracer = new MockTracer(Propagators.TextMap);
            MockSpan parent = (MockSpan)tracer.BuildSpan("parent").Start();

            MockSpan nextSpan = (MockSpan)tracer.BuildSpan("follows")
                .AddReference("a_reference", parent.Context)
                .Start();

            Assert.Equal(parent.Context.SpanId, nextSpan.ParentId);
            Assert.Single(nextSpan.References);
            Assert.Equal(nextSpan.References[0],
                new MockSpan.Reference(parent.Context, "a_reference"));
        }

        [Fact]
        public void TestDefaultConstructor()
        {
            MockTracer mockTracer = new MockTracer();
            IScope scope = mockTracer.BuildSpan("foo").StartActive(true);
            Assert.Equal(scope, mockTracer.ScopeManager.Active);

            var propag = new Dictionary<string, string>();
            mockTracer.Inject(scope.Span.Context, BuiltinFormats.TextMap, new TextMapInjectAdapter(propag));
            Assert.True(propag.Any());
        }

        [Fact]
        public void TestChildOfWithNullParentDoesNotThrowException()
        {
            MockTracer tracer = new MockTracer();
            ISpan parent = null;
            ISpan span = tracer.BuildSpan("foo").AsChildOf(parent).Start();
            span.Finish();
        }
    }
}
