using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace OpenTracing.OpenTracing.Propagation
{
    public class TextMapFormat : ReadOnlyDictionary<string,string>, IFormat
    {
        public TextMapFormat(IDictionary<string, string> properties) : 
            base(properties)
        {
        }
    }
}