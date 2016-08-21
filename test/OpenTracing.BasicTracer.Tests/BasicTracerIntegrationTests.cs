using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
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

            var span = tracer.StartSpan("TestOperation");

            Assert.NotNull(span);
        }

        [Fact]
        public void DefaultBasicTracer_WhenSpanInjectedToMemoryCarrier_Work()
        {
            var tracer = GetTracer();

            var span = tracer.StartSpan("TestOperation");

            var traceId = span.TypedContext().TraceId;
            var spanId = span.TypedContext().SpanId;

            var data = new Dictionary<string, string>();

            tracer.InjectIntoTextMap(span.Context, data);

            Assert.Equal(traceId.ToString(), data["ot-traceid"]);
            Assert.Equal(spanId.ToString(), data["ot-spanid"]);
        }

        [Fact]
        public void DefaultBasicTracer_WhenJoinBadSpanToMemoryCarrier_Fails()
        {
            var tracer = GetTracer();

            var data = new Dictionary<string, string>();

            var spanContext = tracer.ExtractFromTextMap(data);

            Assert.Null(spanContext);
        }

        [Fact]
        public void DefaultBasicTracer_WhenJoinValidSpanToMemoryCarrier_Works()
        {
            var tracer = GetTracer();

            var testTraceId = Guid.NewGuid();
            var testSpanId = Guid.NewGuid();

            var data = new Dictionary<string, string>()
            {
                { "ot-traceid", testTraceId.ToString() },
                { "ot-spanid", testSpanId.ToString() },
            };

            var spanContext = (SpanContext)tracer.ExtractFromTextMap(data);

            Assert.NotNull(spanContext);

            Assert.Equal(testTraceId, spanContext.TraceId);
            Assert.Equal(testSpanId, spanContext.SpanId);
        }

        [Fact]
        public void DefaultBasicTracer_WhenFinishSpan_CallsRecorderWithAllSpanData()
        {
            var recorder = new SimpleMockRecorder();
            var tracer = GetTracer(recorder: recorder);

            var startTimestamp = new DateTime(2016, 1, 1, 12, 0, 0, DateTimeKind.Utc);
            var finishTimestamp = new DateTime(2016, 1, 1, 12, 0, 5, DateTimeKind.Utc);

            var span = tracer.StartSpan("TestOperation", startTimestamp)
                .SetTag("tagkey", "TagValue")
                .SetBaggageItem("baggagekey", "BaggageValue");

            span.Finish(finishTimestamp);

            var recordedSpan = recorder.Spans.First();

            Assert.Equal("TestOperation", recordedSpan.OperationName);
            Assert.Equal(startTimestamp, recordedSpan.StartTimestamp);
            Assert.Equal(TimeSpan.FromSeconds(5), recordedSpan.Duration);

            Assert.Equal("BaggageValue", recordedSpan.Context.GetBaggageItem("baggagekey"));
            Assert.Equal("TagValue", recordedSpan.Tags["tagkey"]);

            Assert.NotEqual(Guid.Empty, recordedSpan.Context.TraceId);
            Assert.NotEqual(Guid.Empty, recordedSpan.Context.SpanId);
        }

        [Fact]
        public async Task Bla()
        {
ITracer tracer = GetTracer();

// create root span
var rootSpan = tracer.StartSpan("get_account")
    .SetBaggageItem("user-id", "1234")
    .SetTagComponent("AspNetCore");

// create HTTP api request
var client = new HttpClient();
var request = new HttpRequestMessage(HttpMethod.Post, "/some_api");
request.Content = new StringContent("{ \"userId\": 1234 }");

// Inject
tracer.InjectIntoHttpHeaders(rootSpan, request.Headers);

// call api
var response = await client.SendAsync(request);

// Extract new context from response
var spanContext = tracer.ExtractFromHttpHeaders(response);

// create new span with reference
var subSpan1 = tracer.StartSpan("sub-operation", SpanReference.ChildOf(spanContext))
    .SetTag("custom-key", "custom-value");

// create another follows_from span with StartOptions
var options = new StartSpanOptions()
    .FollowsFrom(rootSpan)
    .SetStartTimestamp(new DateTime(2016, 8, 21, 14, 0, 0, DateTimeKind.Utc));
var subSpan2 = tracer.StartSpan("sub-operation", options)
    .SetTagHttpUrl("http://example.com/api");

// finish operations
subSpan1.Finish();
subSpan2.Finish(new FinishSpanOptions().SetFinishTimestamp(new DateTime(2016, 8, 21, 14, 0, 5, DateTimeKind.Utc)));
rootSpan.Finish();
        }
    }
}