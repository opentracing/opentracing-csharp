using NUnit.Framework;
using System.Linq;
using System.Collections.Generic;
using OpenTracing.OpenTracing.Tracer;
using OpenTracing.OpenTracing.Propagation;
using OpenTracing.OpenTracing.Span;
using System;
using OpenTracing.BasicTracer.OpenTracingContext;

namespace OpenTracing.BasicTracer.IntegrationTests
{
    [TestFixture()]
    public class BasicTracerIntegrationTests
    {
        [Test()]
        public void DefaultBasicTracer_WhenStartSpanCalled_ReturnsSpan()
        {
            var spanContextFactory = new OpenTracingSpanContextFactory();
            var traceBuilder = new TracerBuilder<OpenTracingContext.OpenTracingSpanContext>();
            traceBuilder.SetSpanContextFactory(spanContextFactory);
            var tracer = traceBuilder.BuildTracer();

            var span = tracer.StartSpan("TestOperation");

            Assert.NotNull(span);
        }

        [Test()]
        public void DefaultBasicTracer_WhenSpanInjectedToMemoryCarrier_Work()
        {
            var spanContextFactory = new OpenTracingSpanContextFactory();
            var traceBuilder = new TracerBuilder<OpenTracingContext.OpenTracingSpanContext>();
            traceBuilder.SetSpanContextFactory(spanContextFactory);
            var tracer = traceBuilder.BuildTracer();

            var span = tracer.StartSpan("TestOperation");

            var traceId = span.GetSpanContext().TraceId;
            var spanId = span.GetSpanContext().SpanId;

            var contextMapper = new OpenTracingSpanContextToTextMapper();
            var memoryCarrier = new MemoryTextMapCarrier<OpenTracingContext.OpenTracingSpanContext>(contextMapper, new Dictionary<string, string>() { });
            tracer.Inject(span, memoryCarrier);

            Assert.AreEqual(traceId.ToString(), memoryCarrier.TextMap["ot-tracer-traceid"]);
            Assert.AreEqual(spanId.ToString(), memoryCarrier.TextMap["ot-tracer-spanid"]);
        }

        [Test()]
        public void DefaultBasicTracer_WhenJoinBadSpanToMemoryCarrier_Fails()
        {
            var spanContextFactory = new OpenTracingSpanContextFactory();
            var traceBuilder = new TracerBuilder<OpenTracingContext.OpenTracingSpanContext>();
            traceBuilder.SetSpanContextFactory(spanContextFactory);
            var tracer = traceBuilder.BuildTracer();

            var contextMapper = new OpenTracingSpanContextToTextMapper();
            var memoryCarrier = new MemoryTextMapCarrier<OpenTracingContext.OpenTracingSpanContext>(contextMapper, new Dictionary<string, string>() { });

            ISpan<OpenTracingContext.OpenTracingSpanContext> span;
            var success = tracer.TryJoin("TestOperation", memoryCarrier, out span);

            Assert.IsFalse(success);
        }

        [Test()]
        public void DefaultBasicTracer_WhenJoinBadSpanToMemoryCarrier_Works()
        {
            var spanContextFactory = new OpenTracingSpanContextFactory();
            var traceBuilder = new TracerBuilder<OpenTracingContext.OpenTracingSpanContext>();
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
            var memoryCarrier = new MemoryTextMapCarrier<OpenTracingContext.OpenTracingSpanContext>(contextMapper, data);

            ISpan<OpenTracingContext.OpenTracingSpanContext> span;
            var success = tracer.TryJoin("TestOperation", memoryCarrier, out span);

            Assert.IsTrue(success);

            var context = span.GetSpanContext();

            Assert.AreEqual(testTraceId.ToString(), memoryCarrier.TextMap["ot-tracer-traceid"]);
            Assert.AreEqual(testSpanId.ToString(), memoryCarrier.TextMap["ot-tracer-spanid"]);
        }

        [Test()]
        public void DefaultBasicTracer_WhenFinishSpan_CallsRecorderWithAllSpanData()
        {
            var spanContextFactory = new OpenTracingSpanContextFactory();
            var traceBuilder = new TracerBuilder<OpenTracingContext.OpenTracingSpanContext>();
            traceBuilder.SetSpanContextFactory(spanContextFactory);
            var simpleMockRecorder = new SimpleMockRecorder();
            traceBuilder.SetSpanRecorder(simpleMockRecorder);
            var tracer = traceBuilder.BuildTracer();

            var span = tracer.StartSpan(new StartSpanOptions<OpenTracingContext.OpenTracingSpanContext>
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

            Assert.AreEqual("TestOperation", simpleMockRecorder.spanEvents.First().OperationName);
            Assert.AreEqual("InitTagValue", simpleMockRecorder.spanEvents.First().Tags["inittagkey"]);
            Assert.AreEqual(DateTime.Parse("2016-01-01 12:00"), simpleMockRecorder.spanEvents.First().StartTime);
            Assert.AreEqual(TimeSpan.FromMinutes(1), simpleMockRecorder.spanEvents.First().Duration);

            Assert.AreEqual("BaggageValue", simpleMockRecorder.spanEvents.First().Context.Baggage["baggagekey"]);
            Assert.AreEqual("TagValue", simpleMockRecorder.spanEvents.First().Tags["tagkey"]);

            Assert.AreEqual(0, simpleMockRecorder.spanEvents.First().Context.ParentId);
            Assert.AreNotEqual(0, simpleMockRecorder.spanEvents.First().Context.TraceId);
            Assert.AreNotEqual(0, simpleMockRecorder.spanEvents.First().Context.SpanId);
        }
    }
}