# Late Span finish example.

This example shows an `ISpan` for a top-level operation, with independent, unknown lifetime, acting as parent of a few asynchronous subtasks. The parent `ISpan` is automatically propagated by `AsyncLocalScopeManager`, so there is no need to reactivate it in the subtasks.

```cs
// Inside the function submitting the subtasks.
// Observe no parent is explicitly specified for task1 nor task2.
Task.Run(async () =>
{
    using (IScope childScope1 = tracer.BuildSpan("task1").StartActive(finishSpanOnDispose:true))
    {
        await Task.Delay(55);
    }
});

Task.Run(async () =>
{
    using (IScope childScope2 = tracer.BuildSpan("task2").StartActive(finishSpanOnDispose:true))
    {
        await Task.Delay(85);
    }
});
```
