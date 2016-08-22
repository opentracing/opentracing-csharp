using OpenTracing.BasicTracer.Context;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace OpenTracing.BasicTracer
{
    public sealed class Span<TContext> : ISpan where TContext : Context.ISpanContext
    {
        private readonly TContext _spanContext;

        private ISpanRecorder<TContext> _spanRecorder;

        private string _operationName;
        private DateTime _startTime;
        private Dictionary<string, string> _tags = new Dictionary<string, string>();
        private List<LogData> _logData = new List<OpenTracing.LogData>();
        private List<SpanReference> _references = new List<SpanReference>();

        public ISpanContext GetSpanContext()
        {
            return _spanContext;
        }

        internal Span(ISpanRecorder<TContext> spanRecorder, TContext spanContext, string operationName, DateTime startTime, List<SpanReference> references)
        {
            _spanContext = spanContext;
            _operationName = operationName;

            _spanRecorder = spanRecorder;

            _startTime = startTime;

            _references = references;
        }

        private bool isFinished = false;

        public void Finish()
        {
            FinishWithOptions(new FinishSpanOptions(DateTime.Now));
        }

        public void FinishWithOptions(FinishSpanOptions finshSpanOptions)
        {
            if (isFinished)
                return;

            var duration = finshSpanOptions.FinishTime - _startTime;

            if (finshSpanOptions.LogData != null)
            {
                _logData.AddRange(finshSpanOptions.LogData);
            }

            var spanData = new SpanData<TContext>()
            {
                Context = _spanContext,
                OperationName = _operationName,
                StartTime = _startTime,
                Duration = duration,
                Tags = _tags,
                LogData = _logData,
                References = _references,
            };

            _spanRecorder.RecordSpan(spanData);
            isFinished = true;
        }

        public void SetTag(string message, string value)
        {
            _tags[message] = value;
        }

        public void SetTag(string message, bool value)
        {
            SetTag(message, value.ToString());
        }

        public void SetTag(string message, int value)
        {
            SetTag(message, value.ToString());
        }

        public void SetBaggageItem(string restrictedKey, string value)
        {
            if (!IsValidBaggaeKey(restrictedKey))
                throw new ArgumentException("Invalid baggage key: '" + restrictedKey + "'");

            _spanContext.SetBaggageItem(restrictedKey.ToLower(), value);
        }

        public string GetBaggageItem(string restrictedKey)
        {
            return _spanContext.GetBaggageItems()[restrictedKey.ToLower()];
        }

        public void Log(LogData logData)
        {
            _logData.Add(logData);
        }

        private bool IsValidBaggaeKey(string key)
        {
            var regEx = new Regex(@"^(?i:[a-z0-9][-a-z0-9]*)$");
            return regEx.IsMatch(key);
        }
    }
}