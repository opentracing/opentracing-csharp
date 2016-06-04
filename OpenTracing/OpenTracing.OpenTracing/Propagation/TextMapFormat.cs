using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace OpenTracing.Propagation
{
    public class TextMapFormat : ReadOnlyDictionary<string,string>
    {
        public TextMapFormat(IDictionary<string, string> properties) : 
            base(properties)
        {
        }
    }
}