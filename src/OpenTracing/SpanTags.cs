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

        public SpanTags Set(string key, string value)
        {
            _span.SetTag(key, value);
            return this;
        }

        public SpanTags Set<T>(string key, T value) where T : struct
        {
            _span.SetTag<T>(key, value);
            return this;
        }
    }
}