using System;
#if NETSTANDARD1_3
using System.Net;
using System.Net.Sockets;
#endif

namespace OpenTracing
{
    public static class SpanTagsExtensions
    {
        /// <summary>
        ///  "span.kind" hints at the relationship between spans, e.g. client/server.
        /// </summary>
        public static SpanTags SpanKind(this SpanTags tags, string spanKind)
        {
            return Set(tags, TagNames.SpanKind, spanKind);
        }

        /// <summary>
        ///  "span.kind" hints at the relationship between spans, e.g. client/server.
        /// </summary>
        public static SpanTags SpanKindServer(this SpanTags tags)
        {
            return SpanKind(tags, "server");
        }

        /// <summary>
        ///  "span.kind" hints at the relationship between spans, e.g. client/server.
        /// </summary>
        public static SpanTags SpanKindClient(this SpanTags tags)
        {
            return SpanKind(tags, "client");
        }

        /// <summary>
        ///  "component" is a low-cardinality identifier of the module, library, or package that is instrumented.
        /// </summary>
        public static SpanTags Component(this SpanTags tags, string component)
        {
            return Set(tags, TagNames.Component, component);
        }



        /// <summary>
        ///  "http.url" records the url of the incoming request.
        /// </summary>
        public static SpanTags HttpUrl(this SpanTags tags, string httpUrl)
        {
            return Set(tags, TagNames.HttpUrl, httpUrl);
        }

        /// <summary>
        ///  "http.url" records the url of the incoming request.
        /// </summary>
        public static SpanTags HttpUrl(this SpanTags tags, Uri httpUrl)
        {
            return Set(tags, TagNames.HttpUrl, httpUrl?.ToString());
        }

        /// <summary>
        ///  "http.method" records the method of the incoming request.
        /// </summary>
        public static SpanTags HttpMethod(this SpanTags tags, string httpMethod)
        {
            return Set(tags, TagNames.HttpUrl, httpMethod);
        }

        /// <summary>
        ///  "http.status_code" records the http status code of the response.
        /// </summary>
        public static SpanTags HttpStatusCode(this SpanTags tags, int httpStatusCode)
        {
            return Set(tags, TagNames.HttpStatusCode, httpStatusCode);
        }

#if NETSTANDARD1_3
        /// <summary>
        ///  "http.status_code" records the http status code of the response.
        /// </summary>
        public static SpanTags HttpStatusCode(this SpanTags tags, HttpStatusCode httpStatusCode)
        {
            return Set(tags, TagNames.HttpStatusCode, httpStatusCode);
        }
#endif

        /// <summary>
        /// "peer.hostname" records the host name of the peer.
        /// </summary>
        public static SpanTags PeerHostname(this SpanTags tags, string peerHostname)
        {
            return Set(tags, TagNames.PeerHostname, peerHostname);
        }

        /// <summary>
        ///  "peer.ipv4" records IPv4 host address of the peer.
        /// </summary>
        public static SpanTags PeerIpV4(this SpanTags tags, string peerIpV4)
        {
            return Set(tags, TagNames.PeerIpV4, peerIpV4);
        }

        /// <summary>
        ///  "peer.ipv6" records the IPv6 host address of the peer.
        /// </summary>
        public static SpanTags PeerIpV6(this SpanTags tags, string peerIpV6)
        {
            return Set(tags, TagNames.PeerIpV6, peerIpV6);
        }

#if NETSTANDARD1_3
        public static SpanTags PeerIp(this SpanTags tags, IPAddress peerIp)
        {
            if (peerIp == null)
            {
                throw new ArgumentNullException(nameof(peerIp));
            }

            if (peerIp.AddressFamily == AddressFamily.InterNetwork)
            {
                return Set(tags, TagNames.PeerIpV4, peerIp.ToString());
            }
            else if (peerIp.AddressFamily == AddressFamily.InterNetworkV6)
            {
                return Set(tags, TagNames.PeerIpV6, peerIp.ToString());
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
        public static SpanTags PeerPort(this SpanTags tags, int peerPort)
        {
            return Set(tags, TagNames.PeerPort, peerPort);
        }

        /// <summary>
        ///  "peer.service" records the service name of the peer.
        /// </summary>
        public static SpanTags PeerService(this SpanTags tags, string peerService)
        {
            return Set(tags, TagNames.PeerService, peerService);
        }

        /// <summary>
        ///  "sampling.priority" determines the priority of sampling this Span.
        /// </summary>
        public static SpanTags SamplingPriority(this SpanTags tags, int samplingPriority)
        {
            return Set(tags, TagNames.SamplingPriority, samplingPriority);
        }

        /// <summary>
        /// "error" indicates whether a Span ended in an error state.
        /// </summary>
        public static SpanTags Error(this SpanTags tags, bool error = true)
        {
            return Set(tags, TagNames.Error, error);
        }


        private static SpanTags Set(SpanTags tags, string key, object value)
        {
            if (tags == null)
            {
                throw new ArgumentNullException(nameof(tags));
            }

            return tags.Set(key, value);
        }
    }
}