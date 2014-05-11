using Common.JetBrainsAnnotations;
using Nancy;
using Nancy.Owin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace NancyServer
{
    /// <summary> http://katanaproject.codeplex.com/wikipage?title=OWIN%20Keys </summary>
    public class KatanaEnvironment : OwinEnvironment
    {
        public KatanaEnvironment([NotNull] NancyContext nancyContext)
            : base(nancyContext)
        {
        }
    }

    /// <summary> http://owin.org/spec/owin-1.0.0.html#_3.2._Environment </summary>
    public class OwinEnvironment
    {
        private readonly IDictionary<string, object> _owinEnvironment;

        public OwinEnvironment([NotNull] NancyContext nancyContext)
        {
            _owinEnvironment = nancyContext.GetOwinEnvironment();
        }


        // Request::
        /// <summary> May be Stream.Null </summary>
        [NotNull]
        public Stream RequestBody { [Pure] get { return (Stream)_owinEnvironment["owin.RequestBody"]; } }
        /// <summary>
        /// The dictionary MUST be mutable.
        /// Keys MUST be HTTP field-names without ':' or whitespace.
        /// Keys MUST be compared using StringComparer.OrdinalIgnoreCase.
        /// All characters in key and value strings SHOULD be within the ASCII codepage.
        /// The value array returned is assumed to be a copy of the data.  Any intended changes to the value array MUST be persisted back to the
        ///   headers dictionary manually by via headers[headerName] = modifiedArray; or headers.Remove(header).
        /// Header values are assumed to be in a mixed format, meaning that a normally comma separated header may appear as a single entry in the
        ///   values array, one entry per value, or a mixture of the two.
        ///   (e.g. new string[1] {“value, value, value”}, new string[3] {“value”, “value”, “value”} or new string[2] {“value, value”, “value”}).
        /// Servers, applications, and intermediaries SHOULD NOT split or merge header values unnecessarily.  While the three formats are supposed
        ///   to be interchangeable, in practice many existing implementations only support one specific format.  Developers should have the
        ///   flexibility to support existing implementations by producing or consuming a selected format without interference.
        /// </summary>
        [NotNull]
        public IDictionary<string, string[]> RequestHeaders { [Pure] get { return (IDictionary<string, string[]>)_owinEnvironment["owin.RequestHeaders"]; } }
        [NotNull]
        public string RequestMethod { [Pure] get { return (string)_owinEnvironment["owin.RequestMethod"]; } }
        [NotNull]
        public string RequestPath { [Pure] get { return (string)_owinEnvironment["owin.RequestPath"]; } }
        [NotNull]
        public string RequestPathBase { [Pure] get { return (string)_owinEnvironment["owin.RequestPathBase"]; } }
        [NotNull]
        public string RequestProtocol { [Pure] get { return (string)_owinEnvironment["owin.RequestProtocol"]; } }
        [NotNull]
        public string RequestQueryString { [Pure] get { return (string)_owinEnvironment["owin.RequestQueryString"]; } }
        [NotNull]
        public string RequestScheme { [Pure] get { return (string)_owinEnvironment["owin.RequestScheme"]; } }


        // Response::
        [NotNull]
        public Stream ResponseBody { [Pure] get { return (Stream)_owinEnvironment["owin.ResponseBody"]; } }
        [NotNull]
        public IDictionary<string, string[]> ResponseHeaders
        { [Pure] get { return (IDictionary<string, string[]>)_owinEnvironment["owin.ResponseHeaders"]; } }
        /// <summary> An optional int containing the HTTP response status code as defined in RFC 2616 section 6.1.1. The default is 200. </summary>
        public int? ResponseStatusCode { [Pure] get { return (int?)_owinEnvironment.TryGetValueOrDefault("owin.ResponseStatusCode"); } }
        /// <summary>
        /// An optional string containing the reason phrase associated the given status code. If none is provided then the server SHOULD
        ///   provide a default as described in RFC 2616 section 6.1.1
        /// </summary>
        [CanBeNull]
        public string ResponseReasonPhrase { [Pure] get { return (string)_owinEnvironment.TryGetValueOrDefault("owin.ResponseReasonPhrase"); } }
        /// <summary>
        /// An optional string containing the protocol name and version (e.g. "HTTP/1.0" or "HTTP/1.1"). If none is provided then the
        ///   “owin.RequestProtocol” key’s value is the default.  
        /// </summary>
        [CanBeNull]
        public string ResponseProtocol { [Pure] get { return (string)_owinEnvironment.TryGetValueOrDefault("owin.ResponseProtocol"); } }
        

        // Other Owin Environment Data::
        /// <summary> A CancellationToken indicating if the request has been cancelled/aborted. </summary>
        public CancellationToken CancellationToken { get { return (CancellationToken)_owinEnvironment["owin.CallCancelled"]; } }
        /// <summary> The string "1.0" indicating OWIN version. </summary>
        [NotNull]
        public string OwinVersion { [Pure] get { return (string)_owinEnvironment["owin.Version"]; } }


        [NotNull]
        public string Uri
        {
            [Pure]
            get
            {
                string uri = "{0}://{1}{2}{3}".Fs(this.RequestScheme, this.RequestHeaders["Host"].First(), this.RequestPathBase, this.RequestPath);

                uri = (this.RequestQueryString != "") ? (uri + "?" + this.RequestQueryString) : uri;

                return uri;
            }
        }

        [Pure, NotNull]
        public string GetRequestMethodWithUri() { return "{0}:: {1}".Fs(this.RequestMethod, this.Uri); }
    }
}