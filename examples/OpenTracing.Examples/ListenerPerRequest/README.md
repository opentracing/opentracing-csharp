# Listener Response example.

This example shows an `ISpan` created upon a message being sent to a `Client`, and its handling along a related, **not shared** `ResponseListener` object with a `OnResponse(String response)` method to finish it.

```cs
private Task<String> Execute(String message, ResponseListener responseListener)
{
    return Task.Run(() =>
    {
        String response = $"{message}:response";
        responseListener.OnResponse(response);
        return response;
    });
}

public Task<String> Send(String message)
{
    ISpan span = tracer.BuildSpan("send").
            WithTag(Tags.SpanKind.Key, Tags.SpanKindClient)
            .Start();
    return Execute(message, new ResponseListener(span));
}
```
