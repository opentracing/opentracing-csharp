# Nested callbacks example.

This example shows a `ISpan` for a top-level operation, and how it can be automatically passed down as the active one on a list of nested callbacks (always one at a time), and finished **only** when the last one executes.

```cs
// Manually activate span so it gets
// propagated as the active one in the nested tasks.
using (tracer.ScopeManager.Activate(span, finishSpanOnDispose:false))
{
    Task.Run(() =>
    {
        tracer.ActiveSpan.SetTag("key1", "1");

        Task.Run(() =>
        {
            tracer.ActiveSpan.SetTag("key2", "2");

            Task.Run(() =>
            {
                tracer.ActiveSpan.SetTag("key3", "3");
                tracer.ActiveSpan.Finish();
            });
            ...
```
