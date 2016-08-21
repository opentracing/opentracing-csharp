using System;

namespace OpenTracing
{
    public class FinishSpanOptions
    {
        public DateTimeOffset? FinishTimestamp { get; set; }

        public FinishSpanOptions SetFinishTimestamp(DateTimeOffset timestamp)
        {
            FinishTimestamp = timestamp;
            return this;
        }
    }
}