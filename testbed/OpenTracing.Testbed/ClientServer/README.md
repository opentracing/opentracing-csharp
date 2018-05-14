# Client-Server example.

This example shows an `ISpan` created by a `Client`, which will send a `Message`/`ISpanContext` to a `Server`, which will in turn extract such context and use it as parent of a new (server-side) `ISpan`.

`Client.Send()` is used to send messages and inject the `ISpanContext` using the `TextMap` format, and `Server.Process()` will process received messages and will extract the context used as parent.

```cs
public void Send()
{
    var message = new Message();

    using (IScope scope = tracer.BuildSpan("send")
            .WithTag(Tags.SpanKind.Key, Tags.SpanKindClient)
            .WithTag(Tags.Component.Key, "example-client")
            .StartActive(finishSpanOnDispose:true))
    {
        tracer.Inject(scope.Span.Context, BuiltinFormats.TextMap, new TextMapInjectAdapter(message));
        queue.Add(message);
    }
}
```
