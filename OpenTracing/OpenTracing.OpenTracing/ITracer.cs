using OpenTracing.Propagation;
using System;

namespace OpenTracing
{
    public interface ITracer
    {
        SpanBuilder BuildSpan(string operationName);
        ISpan StartSpan(StartSpanOptions startSpanOptions);

        void Inject<TFormat>(ISpan span, IInjectCarrier<TFormat> carrier);
        ExtractResult Extract<TFormat>(string operationName, IExtractCarrier<TFormat> carrier);
    }
}
