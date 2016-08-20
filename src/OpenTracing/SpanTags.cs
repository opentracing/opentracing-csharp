using System;

namespace OpenTracing
{
    public class SpanTags
    {
        private readonly ISpan _span;

        public SpanTags(ISpan span)
        {
            if (span == null)
            {
                throw new ArgumentNullException(nameof(span));
            }

            _span = span;
        }

        public ISpan Span => _span;

        public SpanTags Set(string key, object value)
        {
            _span.SetTag(key, value);
            return this;
        }
    }
}