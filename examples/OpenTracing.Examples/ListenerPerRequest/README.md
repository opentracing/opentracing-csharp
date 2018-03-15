# Listener Response example.

This example shows an `ISpan` created upon a message being sent to a `Client`, and its handling along a related, **not shared** `ResponseListener` object with a `OnResponse(string response)` method to finish it.

```cs
private async Task<string> Execute(string message, ResponseListener responseListener)
{
    await Task.Delay(10);

    // send via wire and get response
    string response = $"{message}:response";
    responseListener.OnResponse(response);
    return response;
}

public Task<string> Send(string message)
{
    ISpan span = _tracer.BuildSpan("send")
	    .WithTag(Tags.SpanKind.Key, Tags.SpanKindClient)
	    .Start();
    return Execute(message, new ResponseListener(span));
}
```
