using OpenTracing.OpenTracing.Context;
using OpenTracing.OpenTracing.Tracer;
using System;
using System.Collections.Generic;

namespace OpenTracing.OpenTracing.Span
{
    public interface ISpan<T> where T : ISpanContext
    {
        T GetSpanContext();

        void Finish();
        void FinishWithOptions(DateTime finishTime);

        void SetBaggageItem(string restrictedKey, string value);
        void SetTag(string message, string value);
        void SetTag(string message, int value);
        void SetTag(string message, bool value);
        void Log(string message, object obj);
        void Log(DateTime dateTime, string message, object obj);

        ITracer<T> GetTracer();
    }
}