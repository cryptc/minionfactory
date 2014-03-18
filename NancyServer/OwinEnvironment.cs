using System;
using Nancy;
using Nancy.Owin;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace NancyServer
{
    public class OwinEnvironment
    {
        private readonly IDictionary<string, object> _owinEnvironment;

        public OwinEnvironment(NancyContext nancyContext)
        {
            _owinEnvironment = nancyContext.GetOwinEnvironment();
        }


        // Request::
        public Stream RequestBody { get { return (Stream)_owinEnvironment["owin.RequestBody"]; } }
        public IDictionary<string, string[]> RequestHeaders { get { return (IDictionary<string, string[]>)_owinEnvironment["owin.RequestHeaders"]; } }
        public string RequestMethod { get { return (string)_owinEnvironment["owin.RequestMethod"]; } }
        public string RequestPath { get { return (string)_owinEnvironment["owin.RequestPath"]; } }
        public string RequestPathBase { get { return (string)_owinEnvironment["owin.RequestPathBase"]; } }
        public string RequestProtocol { get { return (string)_owinEnvironment["owin.RequestProtocol"]; } }
        public string RequestQueryString { get { return (string)_owinEnvironment["owin.RequestQueryString"]; } }
        public string RequestScheme { get { return (string)_owinEnvironment["owin.RequestScheme"]; } }

        // Response::
        public Stream ResponseBody { get { return (Stream)_owinEnvironment["owin.ResponseBody"]; } }
        public IDictionary<string, string[]> ResponseHeaders { get { return (IDictionary<string, string[]>)_owinEnvironment["owin.ResponseHeaders"]; } }
        
        public string OwinVersion { get { return (string)_owinEnvironment["owin.Version"]; } }
        public CancellationToken CancellationToken { get { return (CancellationToken)_owinEnvironment["owin.CallCancelled"]; } }
        public string Uri
        {
            get
            {
                string uri = "{0}://{1}{2}{3}".F(this.RequestScheme, this.RequestHeaders["Host"].First(), this.RequestPathBase, this.RequestPath);

                uri = (this.RequestQueryString != "") ? (uri + "?" + this.RequestQueryString) : uri;

                return uri;
            }
        }

        public string GetRequestMethodWithUri()
        {
            return string.Format("{0} {1}", this.RequestMethod, this.Uri);
        }

    }
}

namespace System
{
    public static class StringExtensions
    {
        public static string F(this string formatString, params object[] args)
        {
            return string.Format(formatString, args);
        }
    }
}


namespace NancyServer
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get["/"] = x =>
            {
                var environment = new OwinEnvironment(this.Context);
                return environment.GetRequestMethodWithUri();
            };
        }
    }
}
