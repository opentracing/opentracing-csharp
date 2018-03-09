# Common Request Handler example.

This example shows an `ISpan` used with `RequestHandler`, which is used as a middleware (as in web frameworks) to manage a new `ISpan` per operation through its `BeforeRequest()`/`AfterResponse()` methods.

Since its methods are not guaranteed to be run in the same thread, activation of such `ISpan`s is not done.

```cs
public void BeforeRequest(Object request, Context context)
{
    // we cannot use active span because we don't know in which thread it is executed
    // and we cannot therefore Activate span. thread can come from common thread pool.
    ISpanBuilder spanBuilder = tracer.BuildSpan(OperationName)
            .IgnoreActiveSpan()
            .WithTag(Tags.SpanKind.Key, Tags.SpanKindClient);

    if (parentContext != null)
    {
        spanBuilder.AsChildOf(parentContext);
    }

    context["span"] = spanBuilder.Start();
}

public void AfterResponse(Object response, Context context)
{
    Object spanObject = context["span"];
    if (spanObject is ISpan)
    {
        ((ISpan)spanObject).Finish();
    }
}
```