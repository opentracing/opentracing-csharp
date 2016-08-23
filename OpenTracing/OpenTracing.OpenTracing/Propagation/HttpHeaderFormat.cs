using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace OpenTracing.Propagation
{
    /// <summary>
    /// The HttpHeaderFormat allows for HTTP-header-compatible String->String map encoding 
    /// of SpanContext state for Tracer.inject and Tracer.extract.
    ///
    /// I.e., keys written to the TextMap MUST be suitable for HTTP header keys (which are 
    /// poorly defined but certainly restricted); and similarly for values(i.e., URL-escaped 
    /// and "not too long").
    /// 
    /// Properties with an unsuitable key or value are removed.
    /// </summary>
    public class HttpHeaderFormat : ReadOnlyDictionary<string,string>
    {
        public HttpHeaderFormat(IDictionary<string, string> properties) : 
            base(RemoveInvalidBaggageProperties(properties))
        {
        }

        private static IDictionary<string, string> RemoveInvalidBaggageProperties(IDictionary<string, string> properties)
        {
            var validProperties = new Dictionary<string, string>();

            foreach (var property in properties)
            {
                if (IsValidBaggaeKey(property.Key))
                {
                    validProperties.Add(property.Key, property.Value);   
                }
            }

            return validProperties;
        }

        private static bool IsValidBaggaeKey(string key)
        {
            var regEx = new Regex(@"^(?i:[a-z0-9][-a-z0-9]*)$");
            return regEx.IsMatch(key);
        }
    }
}