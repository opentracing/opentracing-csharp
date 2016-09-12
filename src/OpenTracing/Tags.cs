namespace OpenTracing
{
    /// <summary>
    /// <para>The following span tags are recommended for instrumentors who are trying to capture more
    /// semantic information about the spans. Tracers may expose additional features based on these
    /// standardized data points. Tag names follow a general structure of namespacing.</para>
    /// <para>See http://opentracing.io/data-semantics/ for details.</para>
    /// </summary>
    public static class Tags
    {
        /// <summary>
        ///  "component" is a low-cardinality identifier of the module, library, or package that is instrumented.
        /// </summary>
        public const string Component = "component";

        /// <summary>
        ///  "span.kind" hints at the relationship between spans, e.g. client/server.
        /// </summary>
        public const string SpanKind = "span.kind";

        /// <summary>
        ///  "http.url" records the url of the incoming request.
        /// </summary>
        public const string HttpUrl = "http.url";

        /// <summary>
        ///  "http.method" records the method of the incoming request.
        /// </summary>
        public const string HttpMethod = "http.method";

        /// <summary>
        ///  "http.status_code" records the http status code of the response.
        /// </summary>
        public const string HttpStatusCode = "http.status_code";

        /// <summary>
        /// "peer.hostname" records the host name of the peer.
        /// </summary>
        public const string PeerHostname = "peer.hostname";

        /// <summary>
        ///  "peer.ipv4" records IPv4 host address of the peer.
        /// </summary>
        public const string PeerIpV4 = "peer.ipv4";

        /// <summary>
        ///  "peer.ipv6" records the IPv6 host address of the peer.
        /// </summary>
        public const string PeerIpV6 = "peer.ipv6";

        /// <summary>
        ///  "peer.port" records the port number of the peer.
        /// </summary>
        public const string PeerPort = "peer.port";

        /// <summary>
        ///  "peer.service" records the service name of the peer.
        /// </summary>
        public const string PeerService = "peer.service";

        /// <summary>
        ///  "sampling.priority" determines the priority of sampling this Span.
        /// </summary>
        public const string SamplingPriority = "sampling.priority";

        /// <summary>
        /// "error" indicates whether a Span ended in an error state.
        /// </summary>
        public const string Error = "error";
    }
}