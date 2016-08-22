using System;

namespace OpenTracing
{
    public interface ISpan
    {
        ISpanContext GetSpanContext();

        void Finish();
        void FinishWithOptions(DateTime finishTime);

        void SetBaggageItem(string restrictedKey, string value);
        string GetBaggageItem(string key);
        void SetTag(string message, string value);
        void SetTag(string message, int value);
        void SetTag(string message, bool value);

        void Log(LogData logData);
    }
}