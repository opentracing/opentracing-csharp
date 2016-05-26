using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTracing.OpenTracing.Span
{
    public interface ISpanEvents
    {
        void onStart();

        void onFinished();

        void onBaggage(string key, string value);

        void onLog(string message);

        void onTag(string key, string value);
    }
}
