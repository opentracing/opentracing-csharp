using System;
using System.Collections.Generic;
using System.Text;

namespace OpenTracing.Decorators
{
    public class SpanContextDecorator : ISpanContext
    {
        private readonly ISpanContext _spanContext;

        public SpanContextDecorator(ISpanContext spanContext)
        {
            _spanContext = spanContext;
        }
        public virtual string TraceId => _spanContext.TraceId;

        public virtual string SpanId => _spanContext.SpanId;

        public virtual IEnumerable<KeyValuePair<string, string>> GetBaggageItems() => _spanContext.GetBaggageItems();
    }
}
