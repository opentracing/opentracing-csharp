using System;
using System.Collections.Generic;

namespace OpenTracing.Propagation
{
    public class TextMapCarrier : IInjectCarrier, IExtractCarrier
    {
        public IDictionary<string, string> TextMap { get; }

        public TextMapCarrier(IDictionary<string, string> dictionary)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }

            TextMap = dictionary;
        }
    }
}