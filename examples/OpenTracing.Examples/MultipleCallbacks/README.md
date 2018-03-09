# Multiple callbacks example.

This example shows an `ISpan` created for a top-level operation, covering a set of asynchronous operations (representing callbacks), and have this `ISpan` finished when **all** of them have been executed.

`Client.Send()` is used to create a new asynchronous operation (callback), and in turn every operation both *implicitly* restores the active `ISpan` (by using `AsyncLocalScopeManager`), and creates a child `ISpan` (useful for measuring the performance of each callback). `AsyncLocalScopeManager` will implicitly propagate the context - in this case, the active `ISpan`, for each asynchronous call.

```cs
// Client.Send()
public async Task<string> Send<T>(T message, long milliseconds)
{
    using (IScope scope = tracer.BuildSpan("subtask").StartActive(finishSpanOnDispose:true))
    {
        await Task.Delay(TimeSpan.FromMilliseconds(milliseconds));
    }

    return message + "::response";
}

// Client.Send() call time.
var span = tracer.BuildSpan("parent").Start();
using (IScope scope = tracer.ScopeManager.Activate(span, finishSpanOnDispose:false))
{
    var rand = new Random();
    for (int i = 0; i < tasks.Length; i++)
        tasks[i] = client.Send("task" + i, rand.Next(300));

    // Finish the parent Span only when its children are done.
    Task.WhenAll(tasks).ContinueWith(arg => span.Finish());
}


```
