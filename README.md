[![Gitter chat](http://img.shields.io/badge/gitter-join%20chat%20%E2%86%92-brightgreen.svg)](https://gitter.im/opentracing/public)

# OpenTracing API for .Net including a BasicTracer

This solution includes 2 packages 

    * A .Net platform API for OpenTracing.
    * A basic tracer implementation using the OpenTracing API

## Required Reading

In order to understand the .Net platform API, one should first be familiar with the
[OpenTracing project](http://opentracing.io) and
[terminology](http://opentracing.io/spec/) more generally.

## Contributing

If you would like to contribute code you can do so through GitHub by forking the repository and sending a pull request (on a branch other than `master` or `gh-pages`).

By contributing your code, you agree to license your contribution under the terms of The MIT License (MIT): https://github.com/dawallin/opentracing-csharp/blob/master/LICENSE

## API pointers for those implementing a tracing system

Tracing system implementors may look at the included BasicTracer package to be able to reuse or copy-paste-modify. 

## API compatibility

For the time being, "mild" backwards-incompatible changes may be made without changing the major version number. As OpenTracing and `opentracing-csharp` mature, backwards compatibility will become more of a priority.

## License

All files are released with The MIT License (MIT).