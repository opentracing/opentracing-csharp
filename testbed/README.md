# OpenTracing-C# testbed.

Goal of these examples is to
- test API changes
- use for regression testing
- show common instrumentation patterns

List of patterns:

- [active_span_replacement](OpenTracing.Testbed/ActiveSpanReplacement) - start an isolated task and query for its result in another task/thread
- [client_server](OpenTracing.Testbed/ClientServer) - typical client-server example
- [common_request_handler](OpenTracing.Testbed/CommonRequestHandler) - one request handler for all requests
- [late_span_finish](OpenTracing.Testbed/LateSpanFinish) - late parent span finish
- [listener_per_request](OpenTracing.Testbed/ListenerPerRequest) - one listener per request
- [multiple_callbacks](OpenTracing.Testbed/MultipleCallbacks) - many callbacks/tasks spawned at the same time
- [nested_callbacks](OpenTracing.Testbed/NestedCallbacks) - one callback/task at the time, defined in a pipeline fashion
