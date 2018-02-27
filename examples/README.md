# OpenTracing-C# examples

Goal of these examples is to
- test API changes
- use for regression testing
- show common instrumentation patterns

List of patterns:

- [active_span_replacement](OpenTracing.Examples/ActiveSpanReplacement) - start an isolated task and query for its result in another task/thread
- [client_server](OpenTracing.Examples/ClientServer) - typical client-server example
- [common_request_handler](OpenTracing.Examples/CommonRequestHandler) - one request handler for all requests
- [late_span_finish](OpenTracing.Examples/LateSpanFinish) - late parent span finish
- [listener_per_request](OpenTracing.Examples/ListenerPerRequest) - one listener per request
- [multiple_callbacks](OpenTracing.Examples/MultipleCallbacks) - many callbacks/tasks spawned at the same time
- [nested_callbacks](OpenTracing.Examples/NestedCallbacks) - one callback/task at the time, defined in a pipeline fashion
