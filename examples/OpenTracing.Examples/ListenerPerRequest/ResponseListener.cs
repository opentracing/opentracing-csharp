using System;

namespace OpenTracing.Examples.ListenerPerRequest
{
    // Response listener per request. Executed in a thread different from 'Send' thread
    public class ResponseListener
    {

        private readonly ISpan _span;

        public ResponseListener(ISpan span)
        {
            _span = span;
        }

        // executed when response is received from server. Any thread.
        public void OnResponse(string response) => _span.Finish();
    }
}
