using OpenTracing.BasicTracer.OpenTracingContext;
using System.Collections.Generic;

namespace OpenTracing.BasicTracer.IntegrationTests
{
    public class SimpleMockRecorder : ISpanRecorder<OpenTracingSpanContext>
    {
        public List<SpanData<OpenTracingSpanContext>> spanEvents = new List<SpanData<OpenTracingSpanContext>>() { };

        public void RecordSpan(SpanData<OpenTracingSpanContext> span)
        {
            spanEvents.Add(span);
        }
    }
}
