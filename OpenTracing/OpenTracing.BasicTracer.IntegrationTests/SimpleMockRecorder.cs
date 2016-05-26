using OpenTracing.OpenTracing.Tracer;
using System.Collections.Generic;
using OpenTracing.OpenTracing.Span;

namespace OpenTracing.BasicTracer.IntegrationTests
{
    public class SimpleMockRecorder : ISpanRecorder<BasicSpanContext>
    {
        public List<SpanData<BasicSpanContext>> spanEvents = new List<SpanData<BasicSpanContext>>() { };

        public void RecordSpan(SpanData<BasicSpanContext> span)
        {
            spanEvents.Add(span);
        }
    }
}
