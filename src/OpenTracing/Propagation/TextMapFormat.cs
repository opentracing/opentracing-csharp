using System.Collections.Generic;

namespace OpenTracing.Propagation
{
    /// <summary>
    /// The TextMapFormat allows for arbitrary String->String map encoding of SpanContext 
    /// state for Tracer.inject and Tracer.extract.
    ///
    /// Unlike HTTP_HEADERS, the builtin TEXT_MAP format expresses no constraints on keys 
    /// or values.
    /// </summary>
    public class TextMapFormat : Dictionary<string, string>
    {
        public TextMapFormat(IDictionary<string, string> properties) :
            base(properties)
        {
        }
    }
}