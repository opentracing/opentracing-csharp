using System;

namespace OpenTracing.Examples.ListenerPerRequest
{
    // Response listener per request. Executed in a thread different from 'send' thread
    public class ResponseListener
    {

        private readonly ISpan span;

        public ResponseListener(ISpan span)
        {
            this.span = span;
        }

        // executed when response is received from server. Any thread.
        public void OnResponse(String response) => span.Finish();
    }
}