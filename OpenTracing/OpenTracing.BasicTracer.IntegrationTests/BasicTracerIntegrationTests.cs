﻿using NUnit.Framework;
using OpenTracing.BasicTracer.OpenTracingContext;
using OpenTracing.Propagation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenTracing.BasicTracer.IntegrationTests
{
    [TestFixture()]
    public class BasicTracerIntegrationTests
    {
        [Test()]
        public void DefaultBasicTracer_WhenStartSpanCalled_ReturnsSpan()
        {
            var spanContextFactory = new OpenTracingSpanContextFactory();
            var traceBuilder = new TracerBuilder<OpenTracingSpanContext>();
            traceBuilder.SetSpanContextFactory(spanContextFactory);
            var tracer = traceBuilder.BuildTracer();

            var span = tracer.BuildSpan("TestOperation").Start();

            Assert.NotNull(span);
        }

        [Test()]
        public void DefaultBasicTracer_WhenSpanInjectedToMemoryCarrier_Work()
        {
            var spanContextFactory = new OpenTracingSpanContextFactory();
            var traceBuilder = new TracerBuilder<OpenTracingSpanContext>();
            traceBuilder.SetSpanContextFactory(spanContextFactory);
            var tracer = traceBuilder.BuildTracer();

            var span = tracer.BuildSpan("TestOperation").Start();

            var memoryCarrier = new MemoryTextMapCarrier();
            tracer.Inject(span.GetSpanContext(), memoryCarrier);

            Assert.IsTrue(memoryCarrier.TextMap.ContainsKey("ot-tracer-traceid"));
            Assert.IsTrue(memoryCarrier.TextMap.ContainsKey("ot-tracer-spanid"));

            Assert.IsTrue(ulong.Parse(memoryCarrier.TextMap["ot-tracer-traceid"]) != 0);
            Assert.IsTrue(ulong.Parse(memoryCarrier.TextMap["ot-tracer-spanid"]) != 0);
        }

        [Test()]
        public void DefaultBasicTracer_WhenExtractBadSpanToMemoryCarrier_Fails()
        {
            var spanContextFactory = new OpenTracingSpanContextFactory();
            var traceBuilder = new TracerBuilder<OpenTracingSpanContext>();
            traceBuilder.SetSpanContextFactory(spanContextFactory);
            var tracer = traceBuilder.BuildTracer();

            var memoryCarrier = new MemoryTextMapCarrier(new Dictionary<string, string>() { });

            var extractResult = tracer.Extract("TestOperation", memoryCarrier);

            Assert.IsFalse(extractResult.Success);
        }

        [Test()]
        public void DefaultBasicTracer_WhenExtractValidSpanToMemoryCarrier_Works()
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

            var memoryCarrier = new MemoryTextMapCarrier(data);

            var extractResult = tracer.Extract("TestOperation", memoryCarrier);

            Assert.IsTrue(extractResult.Success);
            Assert.IsTrue(extractResult.SpanContext is OpenTracingSpanContext);

            Assert.AreEqual(testTraceId.ToString(), memoryCarrier.TextMap["ot-tracer-traceid"]);
            Assert.AreEqual(testSpanId.ToString(), memoryCarrier.TextMap["ot-tracer-spanid"]);
        }

        [Test()]
        public void DefaultBasicTracer_WhenFinishSpan_CallsRecorderWithAllSpanData()
        {
            var spanContextFactory = new OpenTracingSpanContextFactory();
            var traceBuilder = new TracerBuilder<OpenTracingSpanContext>();
            traceBuilder.SetSpanContextFactory(spanContextFactory);
            var simpleMockRecorder = new SimpleMockRecorder();
            traceBuilder.SetSpanRecorder(simpleMockRecorder);
            var tracer = traceBuilder.BuildTracer();

            var parentSpan = spanContextFactory.NewRootSpanContext();

            var span = tracer.BuildSpan("TestOperation")
                .WithStartTime(DateTime.Parse("2016-01-01 12:00"))
                .WithTag("inittagkey", "InitTagValue")
                .AsChildOf(parentSpan)
                .Start();

            span.SetBaggageItem("baggagekey", "BaggageValue");
            span.SetTag("tagkey", "TagValue");

            span.FinishWithOptions(new FinishSpanOptions(DateTime.Parse("2016-01-01 12:00") + TimeSpan.FromMinutes(1)));

            Assert.AreEqual("TestOperation", simpleMockRecorder.spanEvents.First().OperationName);
            Assert.AreEqual("InitTagValue", simpleMockRecorder.spanEvents.First().Tags["inittagkey"]);
            Assert.AreEqual(DateTime.Parse("2016-01-01 12:00"), simpleMockRecorder.spanEvents.First().StartTime);
            Assert.AreEqual(TimeSpan.FromMinutes(1), simpleMockRecorder.spanEvents.First().Duration);

            Assert.AreEqual("BaggageValue", simpleMockRecorder.spanEvents.First().Context.Baggage["baggagekey"]);
            Assert.AreEqual("TagValue", simpleMockRecorder.spanEvents.First().Tags["tagkey"]);

            Assert.AreEqual(0, simpleMockRecorder.spanEvents.First().Context.ParentId);
            Assert.AreNotEqual(0, simpleMockRecorder.spanEvents.First().Context.TraceId);
            Assert.AreNotEqual(0, simpleMockRecorder.spanEvents.First().Context.SpanId);

            Assert.AreEqual(SpanReferenceType.ChildOfRef, simpleMockRecorder.spanEvents.First().References.First().Type);
            Assert.AreEqual(parentSpan, simpleMockRecorder.spanEvents.First().References.First().ReferencedContext);
        }
    }
}