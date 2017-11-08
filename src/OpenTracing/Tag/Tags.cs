// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Tags.cs">
//   Copyright 2017-2018 The OpenTracing Authors
//   
//   Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except
//   in compliance with the License. You may obtain a copy of the License at
//   
//   http://www.apache.org/licenses/LICENSE-2.0
// 
//   Unless required by applicable law or agreed to in writing, software distributed under the License
//   is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
//   or implied. See the License for the specific language governing permissions and limitations under
//   the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OpenTracing.Tag
{
    /// <summary>
    ///     The following span tags are recommended for instrumentors who are trying to capture more
    ///     semantic information about the spans. Tracers may expose additional features based on these
    ///     standardized data points. Tag names follow a general structure of namespacing.
    ///     <see href="https://github.com/opentracing/specification/blob/master/semantic_conventions.md" />
    /// </summary>
    internal static class Tags
    {
        /// <summary>
        ///     A static readonlyant for setting the span kind to indicate that it represents a server span.
        /// </summary>
        public static readonly string SpanKindServer = "server";

        /// <summary>
        ///     A static readonlyant for setting the span kind to indicate that it represents a client span.
        /// </summary>
        public static readonly string SpanKindClient = "client";

        /// <summary>
        ///     A static readonlyant for setting the span kind to indicate that it represents a producer span, in a messaging
        ///     scenario.
        /// </summary>
        public static readonly string SpanKindProducer = "producer";

        /// <summary>
        ///     A static readonlyant for setting the span kind to indicate that it represents a consumer span, in a messaging
        ///     scenario.
        /// </summary>
        public static readonly string SpanKindConsumer = "consumer";

        /// <summary>
        ///     HttpUrl records the url of the incoming request.
        /// </summary>
        public static readonly StringTag HttpUrl = new StringTag("http.url");

        /// <summary>
        ///     HttpStatus records the http status code of the response.
        /// </summary>
        public static readonly IntTag HttpStatus = new IntTag("http.status_code");

        /// <summary>
        ///     HttpMethod records the http method. Case-insensitive.
        /// </summary>
        public static readonly StringTag HttpMethod = new StringTag("http.method");

        /// <summary>
        ///     PeerHostIpv4 records IPv4 host address of the peer.
        /// </summary>
        public static readonly IntOrStringTag PeerHostIpv4 = new IntOrStringTag("peer.ipv4");

        /// <summary>
        ///     PeerHostIpv6 records the IPv6 host address of the peer.
        /// </summary>
        public static readonly StringTag PeerHostIpv6 = new StringTag("peer.ipv6");

        /// <summary>
        ///     PeerService records the service name of the peer.
        /// </summary>
        public static readonly StringTag PeerService = new StringTag("peer.service");

        /// <summary>
        ///     PeerHostname records the host name of the peer.
        /// </summary>
        public static readonly StringTag PeerHostname = new StringTag("peer.hostname");

        /// <summary>
        ///     PeerPort records the port number of the peer.
        /// </summary>
        public static readonly IntTag PeerPort = new IntTag("peer.port");

        /// <summary>
        ///     SamplingPriority determines the priority of sampling this Span.
        /// </summary>
        public static readonly IntTag SamplingPriority = new IntTag("sampling.priority");

        /// <summary>
        ///     SpanKind hints at the relationship between spans, e.g. client/server.
        /// </summary>
        public static readonly StringTag SpanKind = new StringTag("span.kind");

        /// <summary>
        ///     Component is a low-cardinality identifier of the module, library, or package that is instrumented.
        /// </summary>
        public static readonly StringTag Component = new StringTag("component");

        /// <summary>
        ///     Error indicates whether a Span ended in an error state.
        /// </summary>
        public static readonly BooleanTag Error = new BooleanTag("error");

        /// <summary>
        ///     DbType indicates the type of Database.
        ///     For any SQL database, "sql". For others, the lower-case database category, e.g. "cassandra", "hbase", or "redis"
        /// </summary>
        public static readonly StringTag DbType = new StringTag("db.type");

        /// <summary>
        ///     DbInstance indicates the instance name of Database.
        ///     If the jdbc.url="jdbc:mysql://127.0.0.1:3306/customers", instance name is "customers".
        /// </summary>
        public static readonly StringTag DbInstance = new StringTag("db.instance");

        /// <summary>
        ///     DbUser indicates the user name of Database, e.g. "readonly_user" or "reporting_user"
        /// </summary>
        public static readonly StringTag DbUser = new StringTag("db.user");

        /// <summary>
        ///     DbStatement records a database statement for the given database type.
        ///     For db.type="SQL", "SELECT /// FROM wuser_table". For db.type="redis", "SET mykey "WuValue".
        /// </summary>
        public static readonly StringTag DbStatement = new StringTag("db.statement");

        /// <summary>
        ///     MessageBusDestination records an address at which messages can be exchanged.
        ///     E.g. A Kafka record has an associated "topic name" that can be extracted by the instrumented
        ///     producer or consumer and stored using this tag.
        /// </summary>
        public static readonly StringTag MessageBusDestination = new StringTag("message_bus.destination");
    }
}