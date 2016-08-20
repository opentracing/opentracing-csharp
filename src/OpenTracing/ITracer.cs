using System;
using OpenTracing.Propagation;

namespace OpenTracing
{
    public interface ITracer
    {
        ISpan StartSpan(string operationName);

        ISpan StartSpan(string operationName, DateTimeOffset startTimestamp);

        void Inject(ISpanContext spanContext, IInjectCarrier carrier);

        ISpanContext Extract(IExtractCarrier carrier);
    }
}
