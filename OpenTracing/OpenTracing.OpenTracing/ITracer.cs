using OpenTracing.Propagation;
using System;

namespace OpenTracing
{
    public interface ITracer
    {
        ISpan StartSpan(string operationName);
        ISpan StartSpan(StartSpanOptions startSpanOptions);

        void Inject<T>(ISpan span, IInjectCarrier<T> carrier);
        ExtractResult Extract<T>(string operationName, IExtractCarrier<T> carrier);
    }
}
