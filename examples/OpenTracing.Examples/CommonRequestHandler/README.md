# Common Request Handler example.

This example shows an `ISpan` used with `RequestHandler`, which is used as a middleware (as in web frameworks) to manage a new `ISpan` per operation through its `BeforeRequest()`/`AfterResponse()` methods.

The active `Span` is not put in the request `Context`, as it will be properly propagated (even between threads) by the `ScopeManager` - which means that, upon having `AfterResponse()` being called, we can simply finish the active `Scope`, if any.

```cs
public void BeforeRequest(object request, Context context)
{
    ISpanBuilder spanBuilder = _tracer.BuildSpan(OperationName)
	    .WithTag(Tags.SpanKind.Key, Tags.SpanKindClient);

    if (_ignoreActiveSpan)
    {
	spanBuilder.IgnoreActiveSpan();
    }

    // No need to put 'span' in Context, as our ScopeManager
    // will automatically propagate it, even when switching between threads,
    // and will be available when AfterResponse() is called.
    spanBuilder.StartActive(true);
}

public void AfterResponse(object response, Context context)
{
    _tracer.ScopeManager.Active.Dispose();
}
```
