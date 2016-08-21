﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenTracing.BasicTracer
{
    public class SpanContextFactory : ISpanContextFactory
    {
        private const short TraceEverything = 100;

        private readonly short _samplingRate;
        private readonly Random _samplingRandom = new Random();

        public SpanContextFactory()
            : this (TraceEverything)
        {
        }

        public SpanContextFactory(short sampleRate)
        {
            if (sampleRate < 0 || sampleRate > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(sampleRate), sampleRate, "Value must be between 0 and 100");
            }

            _samplingRate = sampleRate;
        }

        private bool ShouldSample()
        {
            if (_samplingRate == 100)
            {
                return true;
            }
            
            var random = _samplingRandom.Next(0, 100);
            return random % _samplingRate == 0;
        }

        public SpanContext CreateSpanContext(IList<SpanReference> references)
        {
            var traceId = references?.FirstOrDefault()?.TypedContext()?.TraceId ?? Guid.NewGuid();
            var spanId = Guid.NewGuid();
            var shouldSample = ShouldSample();

            var baggage = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            if (references != null)
            {
                foreach (var reference in references)
                {
                    foreach (var kvp in reference.Context.GetBaggageItems())
                    {
                        baggage[kvp.Key] = kvp.Value;
                    }
                }
            }

            return new SpanContext(traceId, spanId, shouldSample, baggage);
        }
    }
}