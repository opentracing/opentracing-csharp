[![Gitter chat][gitter-img]][gitter] [![Build Status][ci-img]][ci]

# OpenTracing API for .NET

This solution includes the .NET platform API for OpenTracing.

## Required Reading

In order to understand the .NET platform API, one must first be familiar with the
[OpenTracing project](http://opentracing.io) and
[terminology](http://opentracing.io/documentation/pages/spec) more specifically.

## Usage

### Initialization

Initialization is OpenTracing-implementation-specific. Generally speaking, the pattern is to initialize a `ITracer` once for the entire process and to use that `ITracer` for the remainder of the process lifetime. It is a best practice to _set_ the [GlobalTracer](https://github.com/opentracing/opentracing-csharp/blob/master/src/OpenTracing/Util/GlobalTracer.cs), even if also making use of cleaner, more modern dependency injection. (See the next section below for rationale)

### Accessing the `ITracer`

Where possible, use some form of dependency injection (of which there are many) to access the `ITracer` instance. For vanilla application code, this is often reasonable and cleaner for all of the usual DI reasons.

That said, instrumentation for packages that are themselves statically configured (e.g., ODBC drivers) may be unable to make use of said DI mechanisms for `ITracer` access, and as such they should fall back on [GlobalTracer](https://github.com/opentracing/opentracing-csharp/blob/master/src/OpenTracing/Util/GlobalTracer.cs). By and large, OpenTracing instrumentation should always allow the programmer to specify a `ITracer` instance to use for instrumentation, though the [GlobalTracer](https://github.com/opentracing/opentracing-csharp/blob/master/src/OpenTracing/Util/GlobalTracer.cs) is a reasonable fallback or default value.

### Scopes and within-process propagation

For any thread, at most one `ISpan` may be "active". Of course there may be many other spans involved with the thread which are (a) started, (b) not finished, and yet (c) not "active": perhaps they are waiting for I/O, blocked on a child span, or otherwise off of the critical path.

It's inconvenient to pass an active `ISpan` from function to function manually, so OpenTracing requires that every `ITracer` contains a `IScopeManager` that grants access to the active `ISpan` through a `IScope`. Any `ISpan` may be transferred to another callback or thread, but not `IScope`; more on this below.

#### Accessing the active Span through `IScope`

Access to the active span is straightforward:

```cs
OpenTracing.ITracer tracer = ...;
...
IScope scope = tracer.ScopeManager.Active;
if (scope != null) {
    scope.Span.Log("...");
}
```

### Starting a new Span

The common case starts a `IScope` that's automatically registered for intra-process propagation via `IScopeManager`.

Note that `StartActive(finishSpanOnDispose: true)` finishes the span on `IScope.Dispose()`.

```cs
OpenTracing.ITracer tracer = ...;
...
using (IScope scope = tracer.BuildSpan("someWork").StartActive(finishSpanOnDispose: true))
{
    try
    {
        // Do things.
    }
    catch (Exception ex)
    {
        Tags.Error.Set(scope.Span, true);
    }

    // No need to call scope.Span.Finish() as we've set finishSpanOnDispose:true in StartActive.
}
```

**If there is a `IScope`, it will act as the parent to any newly started `ISpan`** unless
the programmer invokes `IgnoreActiveSpan()` at `BuildSpan()` time or specified parent context explicitly:

```cs
OpenTracing.ITracer tracer = ...;
...
IScope scope = tracer.BuildSpan("someWork").IgnoreActiveSpan().StartActive(finishSpanOnDispose: true);
```

### Using scopes with `async/await`

OpenTracing contains an `IScopeManager` implementation that uses `AsyncLocal` to flow spans with the execution. It is therefore possible to use scopes and spans with `async/await`:

```cs
OpenTracing.ITracer tracer = ...;
...
using (IScope parentScope = tracer.BuildSpan("Parent").StartActive(finishSpanOnDispose: true))
{
    await SomeAsynchronousWork();

    // It's still possible to access the current span
    parentScope.Span.Log(...);

    // The child scope will automatically use parentScope as its parent.
    using (IScope childScope = tracer.BuildSpan("Child").StartActive(finishSpanOnDispose: true))
    {
        childScope.Span.Log(...);

        await SomeMoreAsynchronousWork();

        childScope.Span.Log(...);
    }
}

public async Task SomeAsynchronousWork()
{
    // use ITracer.ActiveSpan to access the current span - which will be "parentScope.Span".
    tracer.ActiveSpan.Log(...);

    await SomeExternalCall();
}

public async Task SomeMoreAsynchronousWork()
{
    // The active span in this case will be "childScope.Span".
    tracer.ActiveSpan.Log(...);

    await SomeExternalCall();
}
```

## Contributing

If you would like to contribute code you can do so through GitHub by forking the repository and sending a pull request.

By contributing your code, you agree to license your contribution under the terms of the APLv2: https://github.com/opentracing/opentracing-csharp/blob/master/LICENSE

## License

All files are released with the Apache 2.0 license.

  [gitter-img]: http://img.shields.io/badge/gitter-join%20chat%20%E2%86%92-brightgreen.svg
  [gitter]: https://gitter.im/opentracing/public
  [ci-img]: https://travis-ci.org/opentracing/opentracing-csharp.svg?branch=master
  [ci]: https://travis-ci.org/opentracing/opentracing-csharp
