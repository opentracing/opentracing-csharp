using OpenTracing.BasicTracer.Context;
using OpenTracing.Propagation;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace OpenTracing.BasicTracer.IntegrationTests
{
    public class BasicTracerIntegrationTests
    {
        private Tracer GetTracer(ISpanRecorder recorder = null)
        {
            var spanRecorder = recorder ?? new SimpleMockRecorder();
            var spanContextFactory = new SpanContextFactory();

            return new Tracer(spanContextFactory, spanRecorder);
        }

        [Fact]
        public void DefaultBasicTracer_WhenStartSpanCalled_ReturnsSpan()
        {
            var tracer = GetTracer();

            var span = tracer.BuildSpan("TestOperation").Start();

            Assert.NotNull(span);
        }

        [Fact]
        public void DefaultBasicTracer_WhenSpanInjectedToMemoryCarrier_Work()
        {
            var tracer = GetTracer();

            var span = tracer.BuildSpan("TestOperation").Start();

            var traceId = span.TypedContext().TraceId;
            var spanId = span.TypedContext().SpanId;

            var dictionaryCarrier = new MemoryTextMapCarrier();

            tracer.Inject(span.Context, dictionaryCarrier);

            Assert.Equal(traceId.ToString(), dictionaryCarrier.TextMap["ot-tracer-traceid"]);
            Assert.Equal(spanId.ToString(), dictionaryCarrier.TextMap["ot-tracer-spanid"]);
        }

        [Fact]
        public void DefaultBasicTracer_WhenJoinBadSpanToMemoryCarrier_Fails()
        {
            var tracer = GetTracer();

            var dictionaryCarrier = new MemoryTextMapCarrier();

            var spanContext = tracer.Extract(dictionaryCarrier);

            Assert.Null(spanContext);
        }

        [Fact]
        public void DefaultBasicTracer_WhenJoinValidSpanToMemoryCarrier_Works()
        {
            var tracer = GetTracer();

            var testTraceId = (ulong)123;
            var testParentId = (ulong)456;
            var testSpanId = (ulong)789;

            var data = new Dictionary<string, string>()
            {
                { "ot-tracer-traceid", testTraceId.ToString() },
                { "ot-tracer-spanid", testSpanId.ToString() },
                { "ot-tracer-parentid", testParentId.ToString() },
            };

            var dictionaryCarrier = new MemoryTextMapCarrier(data);

            var spanContext = (SpanContext)tracer.Extract(dictionaryCarrier);

            Assert.NotNull(spanContext);

            Assert.Equal(testTraceId, spanContext.TraceId);
            Assert.Equal(testSpanId, spanContext.SpanId);
            Assert.Equal(testParentId, spanContext.ParentId);
        }

        [Fact]
        public void DefaultBasicTracer_WhenFinishSpan_CallsRecorderWithAllSpanData()
        {
            var recorder = new SimpleMockRecorder();
            var tracer = GetTracer(recorder: recorder);

            var startTimestamp = new DateTime(2016, 1, 1, 12, 0, 0, DateTimeKind.Utc);
            var finishTimestamp = new DateTime(2016, 1, 1, 12, 0, 5, DateTimeKind.Utc);

            var span = tracer.BuildSpan("TestOperation")
                .WithStartTimestamp(startTimestamp)
                .Start()
                .SetTag("tagkey", "TagValue")
                .SetBaggageItem("baggagekey", "BaggageValue");

            span.Finish(finishTimestamp);

            var recordedSpan = recorder.Spans.First();

            Assert.Equal("TestOperation", recordedSpan.OperationName);
            Assert.Equal(startTimestamp, recordedSpan.StartTimestamp);
            Assert.Equal(TimeSpan.FromSeconds(5), recordedSpan.Duration);

            Assert.Equal("BaggageValue", recordedSpan.Context.GetBaggageItem("baggagekey"));
            Assert.Equal("TagValue", recordedSpan.Tags["tagkey"]);
        }
    }
}