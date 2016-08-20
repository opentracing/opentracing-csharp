using System;
using System.Collections.Generic;
using System.Linq;
using OpenTracing.Propagation;
using OpenTracing.BasicTracer.OpenTracingContext;
using Xunit;

namespace OpenTracing.BasicTracer.IntegrationTests
{
    public class BasicTracerIntegrationTests
    {
        [Fact]
        public void DefaultBasicTracer_WhenStartSpanCalled_ReturnsSpan()
        {
            var spanContextFactory = new OpenTracingSpanContextFactory();
            var traceBuilder = new TracerBuilder<OpenTracingSpanContext>();
            traceBuilder.SetSpanContextFactory(spanContextFactory);
            var tracer = traceBuilder.BuildTracer();

            var span = tracer.StartSpan("TestOperation");

            Assert.NotNull(span);
        }

        [Fact]
        public void DefaultBasicTracer_WhenSpanInjectedToMemoryCarrier_Work()
        {
            var spanContextFactory = new OpenTracingSpanContextFactory();
            var traceBuilder = new TracerBuilder<OpenTracingSpanContext>();
            traceBuilder.SetSpanContextFactory(spanContextFactory);
            var tracer = traceBuilder.BuildTracer();

            var span = tracer.StartSpan("TestOperation");

            var traceId = span.GetSpanContext().TraceId;
            var spanId = span.GetSpanContext().SpanId;

            var contextMapper = new OpenTracingSpanContextToTextMapper();
            var memoryCarrier = new MemoryTextMapCarrier<OpenTracingSpanContext>(contextMapper, new Dictionary<string, string>() { });
            tracer.Inject(span, memoryCarrier);

            Assert.Equal(traceId.ToString(), memoryCarrier.TextMap["ot-tracer-traceid"]);
            Assert.Equal(spanId.ToString(), memoryCarrier.TextMap["ot-tracer-spanid"]);
        }

        [Fact]
        public void DefaultBasicTracer_WhenJoinBadSpanToMemoryCarrier_Fails()
        {
            var spanContextFactory = new OpenTracingSpanContextFactory();
            var traceBuilder = new TracerBuilder<OpenTracingSpanContext>();
            traceBuilder.SetSpanContextFactory(spanContextFactory);
            var tracer = traceBuilder.BuildTracer();

            var contextMapper = new OpenTracingSpanContextToTextMapper();
            var memoryCarrier = new MemoryTextMapCarrier<OpenTracingSpanContext>(contextMapper, new Dictionary<string, string>() { });

            ISpan<OpenTracingContext.OpenTracingSpanContext> span;
            var success = tracer.TryJoin("TestOperation", memoryCarrier, out span);

            Assert.False(success);
        }

        [Fact]
        public void DefaultBasicTracer_WhenJoinValidSpanToMemoryCarrier_Works()
        {
            var spanContextFactory = new OpenTracingSpanContextFactory();
            var traceBuilder = new TracerBuilder<OpenTracingSpanContext>();
            traceBuilder.SetSpanContextFactory(spanContextFactory);
            var tracer = traceBuilder.BuildTracer();

            var testTraceId = 1234;
            var testSpanId = 9876;

            var data = new Dictionary<string, string>()
            {
                { "ot-tracer-traceid", testTraceId.ToString() },
                { "ot-tracer-spanid", testSpanId.ToString() },
            };

            var contextMapper = new OpenTracingSpanContextToTextMapper();
            var memoryCarrier = new MemoryTextMapCarrier<OpenTracingSpanContext>(contextMapper, data);

            ISpan<OpenTracingContext.OpenTracingSpanContext> span;
            var success = tracer.TryJoin("TestOperation", memoryCarrier, out span);

            Assert.True(success);

            var context = span.GetSpanContext();

            Assert.Equal(testTraceId.ToString(), memoryCarrier.TextMap["ot-tracer-traceid"]);
            Assert.Equal(testSpanId.ToString(), memoryCarrier.TextMap["ot-tracer-spanid"]);
        }

        [Fact]
        public void DefaultBasicTracer_WhenFinishSpan_CallsRecorderWithAllSpanData()
        {
            var spanContextFactory = new OpenTracingSpanContextFactory();
            var traceBuilder = new TracerBuilder<OpenTracingSpanContext>();
            traceBuilder.SetSpanContextFactory(spanContextFactory);
            var simpleMockRecorder = new SimpleMockRecorder();
            traceBuilder.SetSpanRecorder(simpleMockRecorder);
            var tracer = traceBuilder.BuildTracer();

            var span = tracer.StartSpan(new StartSpanOptions()
            {
                OperationName = "TestOperation",
                StartTime = DateTime.Parse("2016-01-01 12:00"),
                Tag = new Dictionary<string, string>
                {
                    { "inittagkey", "InitTagValue" },
                },
            });

            span.SetBaggageItem("baggagekey", "BaggageValue");
            span.SetTag("tagkey", "TagValue");

            span.FinishWithOptions(DateTime.Parse("2016-01-01 12:00") + TimeSpan.FromMinutes(1));

            Assert.Equal("TestOperation", simpleMockRecorder.spanEvents.First().OperationName);
            Assert.Equal("InitTagValue", simpleMockRecorder.spanEvents.First().Tags["inittagkey"]);
            Assert.Equal(DateTime.Parse("2016-01-01 12:00"), simpleMockRecorder.spanEvents.First().StartTime);
            Assert.Equal(TimeSpan.FromMinutes(1), simpleMockRecorder.spanEvents.First().Duration);

            Assert.Equal("BaggageValue", simpleMockRecorder.spanEvents.First().Context.Baggage["baggagekey"]);
            Assert.Equal("TagValue", simpleMockRecorder.spanEvents.First().Tags["tagkey"]);

            Assert.Equal((ulong)0, simpleMockRecorder.spanEvents.First().Context.ParentId);
            Assert.NotEqual((ulong)0, simpleMockRecorder.spanEvents.First().Context.TraceId);
            Assert.NotEqual((ulong)0, simpleMockRecorder.spanEvents.First().Context.SpanId);
        }
    }
}