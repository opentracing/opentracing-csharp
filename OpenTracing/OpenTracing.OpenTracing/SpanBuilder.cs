using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTracing
{
    public class SpanBuilder
    {
        private ISpanFactory _spanFactory;

        private string _operationName;
        private DateTime? _startTime;
        private Dictionary<string, string> _tags { get; set; } = new Dictionary<string, string>() { };

        private List<SpanReference> _references = new List<SpanReference> { };

        public SpanBuilder(ISpanFactory spanFactory, string operationName)
        {
            _spanFactory = spanFactory;
            _operationName = operationName;
        }

        public ISpan Start()
        {
            return _spanFactory.StartSpan(
                new StartSpanOptions()
                {
                    OperationName = _operationName,
                    StartTime = _startTime ?? DateTime.Now,
                    Tag = _tags,
                    References = _references,
                });
        }
    }
}
