using System;
using System.Net;

namespace sppenyakitlambung.Models
{
    public class HttpRequest<TResult> : HttpRequest
    {
        public HttpRequest(string url) : base(url) { }
        public TResult Result;
    }

    public class HttpRequest
    {
        public HttpRequest(string url)
        {
            Url = url;
        }

        public HttpStatusCode HttpStatusCode;
        public string HttpResponseContent;
        public bool Successful;
        public string Url;
    } 
}
