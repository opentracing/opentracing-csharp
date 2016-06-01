using System.Collections.Generic;
using OpenTracing.BasicTracer.Span;
using OpenTracing.BasicTracer.Tracer;

namespace OpenTracing.BasicTracer.IntegrationTests
{
    public class SimpleMockRecorder : ISpanRecorder<OpenTracingContext.OpenTracingSpanContext>
    {
        public List<SpanData<OpenTracingContext.OpenTracingSpanContext>> spanEvents = new List<SpanData<OpenTracingContext.OpenTracingSpanContext>>() { };

        public void RecordSpan(SpanData<OpenTracingContext.OpenTracingSpanContext> span)
        {
            spanEvents.Add(span);
        }
    }
}
