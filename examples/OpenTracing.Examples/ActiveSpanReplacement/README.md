# Active Span replacement example.

This example shows an `ISpan` being created and then passed to an asynchronous task, which will temporary activate it to finish its processing, and further restore the previously active `ISpan`.

```cs
// Create a new Span for this task
using (IScope taskScope = tracer.BuildSpan("task").StartActive(finishSpanOnDispose:true))
{
    // Simulate work strictly related to the initial Span
    // and finish it.
    using (IScope initialScope = tracer.ScopeManager.Activate(initialSpan, finishSpanOnDispose:true))
    {
        await Task.Delay(50);
    }
}
```
