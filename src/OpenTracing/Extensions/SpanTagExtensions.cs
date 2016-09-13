using System;
using System.Net;
using System.Net.Http;
#if NETSTANDARD1_3
using System.Net.Sockets;
#endif

namespace OpenTracing
{
    /// <summary>
    /// Typed extension methods for common tags.
    /// </summary>
    public static class TagExtensions
    {
        /// <summary>
        ///  "span.kind" hints at the relationship between spans, e.g. client/server.
        /// </summary>
        public static ISpan SetTagSpanKind(this ISpan span, string spanKind)
        {
            return SetTag(span, Tags.SpanKind, spanKind);
        }

        /// <summary>
        ///  "span.kind" hints at the relationship between spans, e.g. client/server.
        /// </summary>
        public static ISpanBuilder WithTagSpanKind(this ISpanBuilder builder, string spanKind)
        {
            return WithTag(builder, Tags.SpanKind, spanKind);
        }

        /// <summary>
        ///  "span.kind" hints at the relationship between spans, e.g. client/server.
        /// </summary>
        public static ISpan SetTagSpanKindServer(this ISpan span)
        {
            return SetTagSpanKind(span, Tags.SpanKindServer);
        }

        /// <summary>
        ///  "span.kind" hints at the relationship between spans, e.g. client/server.
        /// </summary>
        public static ISpanBuilder WithTagSpanKindServer(this ISpanBuilder builder)
        {
            return WithTagSpanKind(builder, Tags.SpanKindServer);
        }

        /// <summary>
        ///  "span.kind" hints at the relationship between spans, e.g. client/server.
        /// </summary>
        public static ISpan SetTagSpanKindClient(this ISpan span)
        {
            return SetTagSpanKind(span, Tags.SpanKindClient);
        }

        /// <summary>
        ///  "span.kind" hints at the relationship between spans, e.g. client/server.
        /// </summary>
        public static ISpanBuilder WithTagSpanKindClient(this ISpanBuilder builder)
        {
            return WithTagSpanKind(builder, Tags.SpanKindClient);
        }

        /// <summary>
        ///  "component" is a low-cardinality identifier of the module, library, or package that is instrumented.
        /// </summary>
        public static ISpan SetTagComponent(this ISpan span, string component)
        {
            return SetTag(span, Tags.Component, component);
        }

        /// <summary>
        ///  "component" is a low-cardinality identifier of the module, library, or package that is instrumented.
        /// </summary>
        public static ISpanBuilder WithTagComponent(this ISpanBuilder builder, string component)
        {
            return WithTag(builder, Tags.Component, component);
        }

        /// <summary>
        ///  "http.url" records the url of the incoming request.
        /// </summary>
        public static ISpan SetTagHttpUrl(this ISpan span, string httpUrl)
        {
            return SetTag(span, Tags.HttpUrl, httpUrl);
        }

        /// <summary>
        ///  "http.url" records the url of the incoming request.
        /// </summary>
        public static ISpanBuilder WithTagHttpUrl(this ISpanBuilder builder, string httpUrl)
        {
            return WithTag(builder, Tags.HttpUrl, httpUrl);
        }

        /// <summary>
        ///  "http.url" records the url of the incoming request.
        /// </summary>
        public static ISpan SetTagHttpUrl(this ISpan span, Uri httpUrl)
        {
            return SetTag(span, Tags.HttpUrl, httpUrl?.ToString());
        }

        /// <summary>
        ///  "http.url" records the url of the incoming request.
        /// </summary>
        public static ISpanBuilder WithTagHttpUrl(this ISpanBuilder builder, Uri httpUrl)
        {
            return WithTag(builder, Tags.HttpUrl, httpUrl?.ToString());
        }

        /// <summary>
        ///  "http.method" records the method of the incoming request.
        /// </summary>
        public static ISpan SetTagHttpMethod(this ISpan span, string httpMethod)
        {
            return SetTag(span, Tags.HttpUrl, httpMethod);
        }

        /// <summary>
        ///  "http.method" records the method of the incoming request.
        /// </summary>
        public static ISpanBuilder WithTagHttpMethod(this ISpanBuilder builder, string httpMethod)
        {
            return WithTag(builder, Tags.HttpUrl, httpMethod);
        }

        /// <summary>
        ///  "http.method" records the method of the incoming request.
        /// </summary>
        public static ISpan SetTagHttpMethod(this ISpan span, HttpMethod httpMethod)
        {
            return SetTag(span, Tags.HttpUrl, httpMethod?.Method);
        }

        /// <summary>
        ///  "http.method" records the method of the incoming request.
        /// </summary>
        public static ISpanBuilder WithTagHttpMethod(this ISpanBuilder builder, HttpMethod httpMethod)
        {
            return WithTag(builder, Tags.HttpUrl, httpMethod?.Method);
        }

        /// <summary>
        ///  "http.status_code" records the http status code of the response.
        /// </summary>
        public static ISpan SetTagHttpStatusCode(this ISpan span, int httpStatusCode)
        {
            return SetTag(span, Tags.HttpStatusCode, httpStatusCode);
        }

        /// <summary>
        ///  "http.status_code" records the http status code of the response.
        /// </summary>
        public static ISpanBuilder WithTagHttpStatusCode(this ISpanBuilder builder, int httpStatusCode)
        {
            return WithTag(builder, Tags.HttpStatusCode, httpStatusCode);
        }

        /// <summary>
        ///  "http.status_code" records the http status code of the response.
        /// </summary>
        public static ISpan SetTagHttpStatusCode(this ISpan span, HttpStatusCode httpStatusCode)
        {
            return SetTag(span, Tags.HttpStatusCode, httpStatusCode);
        }

        /// <summary>
        ///  "http.status_code" records the http status code of the response.
        /// </summary>
        public static ISpanBuilder WithTagHttpStatusCode(this ISpanBuilder builder, HttpStatusCode httpStatusCode)
        {
            return WithTag(builder, Tags.HttpStatusCode, httpStatusCode);
        }

        /// <summary>
        /// "peer.hostname" records the host name of the peer.
        /// </summary>
        public static ISpan SetTagPeerHostname(this ISpan span, string peerHostname)
        {
            return SetTag(span, Tags.PeerHostname, peerHostname);
        }

        /// <summary>
        /// "peer.hostname" records the host name of the peer.
        /// </summary>
        public static ISpanBuilder WithTagPeerHostname(this ISpanBuilder builder, string peerHostname)
        {
            return WithTag(builder, Tags.PeerHostname, peerHostname);
        }

        /// <summary>
        ///  "peer.ipv4" records IPv4 host address of the peer.
        /// </summary>
        public static ISpan SetTagPeerIpV4(this ISpan span, string peerIpV4)
        {
            return SetTag(span, Tags.PeerIpV4, peerIpV4);
        }

        /// <summary>
        ///  "peer.ipv4" records IPv4 host address of the peer.
        /// </summary>
        public static ISpanBuilder WithTagPeerIpV4(this ISpanBuilder builder, string peerIpV4)
        {
            return WithTag(builder, Tags.PeerIpV4, peerIpV4);
        }

        /// <summary>
        ///  "peer.ipv6" records the IPv6 host address of the peer.
        /// </summary>
        public static ISpan SetTagPeerIpV6(this ISpan span, string peerIpV6)
        {
            return SetTag(span, Tags.PeerIpV6, peerIpV6);
        }

        /// <summary>
        ///  "peer.ipv6" records the IPv6 host address of the peer.
        /// </summary>
        public static ISpanBuilder WithTagPeerIpV6(this ISpanBuilder builder, string peerIpV6)
        {
            return WithTag(builder, Tags.PeerIpV6, peerIpV6);
        }

#if NETSTANDARD1_3
        
        /// <summary>
        /// Sets either "peer.ipv4" or "peer.ipv6".
        /// </summary>
        public static ISpan SetTagPeerIp(this ISpan span, IPAddress peerIp)
        {
            if (peerIp == null)
            {
                throw new ArgumentNullException(nameof(peerIp));
            }

            if (peerIp.AddressFamily == AddressFamily.InterNetwork)
            {
                return SetTag(span, Tags.PeerIpV4, peerIp.ToString());
            }
            else if (peerIp.AddressFamily == AddressFamily.InterNetworkV6)
            {
                return SetTag(span, Tags.PeerIpV6, peerIp.ToString());
            }
            else
            {
                throw new NotSupportedException($"{nameof(peerIp.AddressFamily)} value '{peerIp.AddressFamily}' is not supported");
            }
        }

        /// <summary>
        /// Sets either "peer.ipv4" or "peer.ipv6".
        /// </summary>
        public static ISpanBuilder WithTagPeerIp(this ISpanBuilder builder, IPAddress peerIp)
        {
            if (peerIp == null)
            {
                throw new ArgumentNullException(nameof(peerIp));
            }

            if (peerIp.AddressFamily == AddressFamily.InterNetwork)
            {
                return WithTag(builder, Tags.PeerIpV4, peerIp.ToString());
            }
            else if (peerIp.AddressFamily == AddressFamily.InterNetworkV6)
            {
                return WithTag(builder, Tags.PeerIpV6, peerIp.ToString());
            }
            else
            {
                throw new NotSupportedException($"{nameof(peerIp.AddressFamily)} value '{peerIp.AddressFamily}' is not supported");
            }
        }

#endif

        /// <summary>
        ///  "peer.port" records the port number of the peer.
        /// </summary>
        public static ISpan SetTagPeerPort(this ISpan span, int peerPort)
        {
            return SetTag(span, Tags.PeerPort, peerPort);
        }

        /// <summary>
        ///  "peer.port" records the port number of the peer.
        /// </summary>
        public static ISpanBuilder WithTagPeerPort(this ISpanBuilder builder, int peerPort)
        {
            return WithTag(builder, Tags.PeerPort, peerPort);
        }

        /// <summary>
        ///  "peer.service" records the service name of the peer.
        /// </summary>
        public static ISpan SetTagPeerService(this ISpan span, string peerService)
        {
            return SetTag(span, Tags.PeerService, peerService);
        }

        /// <summary>
        ///  "peer.service" records the service name of the peer.
        /// </summary>
        public static ISpanBuilder WithTagPeerService(this ISpanBuilder builder, string peerService)
        {
            return WithTag(builder, Tags.PeerService, peerService);
        }

        /// <summary>
        ///  "sampling.priority" determines the priority of sampling this Span.
        /// </summary>
        public static ISpan SetTagSamplingPriority(this ISpan span, int samplingPriority)
        {
            return SetTag(span, Tags.SamplingPriority, samplingPriority);
        }

        /// <summary>
        ///  "sampling.priority" determines the priority of sampling this Span.
        /// </summary>
        public static ISpanBuilder WithTagSamplingPriority(this ISpanBuilder builder, int samplingPriority)
        {
            return WithTag(builder, Tags.SamplingPriority, samplingPriority);
        }

        /// <summary>
        /// "error" indicates whether a Span ended in an error state.
        /// </summary>
        public static ISpan SetTagError(this ISpan span, bool error = true)
        {
            return SetTag(span, Tags.Error, error);
        }

        /// <summary>
        /// "error" indicates whether a Span ended in an error state.
        /// </summary>
        public static ISpanBuilder WithTagError(this ISpanBuilder builder, bool error = true)
        {
            return WithTag(builder, Tags.Error, error);
        }

        private static ISpanBuilder WithTag(ISpanBuilder builder, string key, object value)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            return builder.WithTag(key, value);
        }

        private static ISpan SetTag(ISpan span, string key, object value)
        {
            if (span == null)
            {
                throw new ArgumentNullException(nameof(span));
            }

            return span.SetTag(key, value);
        }
    }
}