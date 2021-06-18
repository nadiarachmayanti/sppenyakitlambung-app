using System;
using System.Collections.Generic;
using System.Net;

namespace sppenyakitlambung.Utilities.Helper
{
    public interface IHttpRequest { }

    public interface IBaseHttpRequest : IHttpRequest { }

    public class BaseHttpRequest : IBaseHttpRequest { }

    public interface IHttpResponse
    {
        HttpStatusCode StatusCode { get; set; }
        string Message { get; set; }
    }

    public interface IListBaseHttpResponse<T> : IHttpResponse
    {
        List<T> Result { get; set; }
    }

    public interface IBaseHttpResponse<T> : IHttpResponse
    {
        T Result { get; set; }
    }

    public class ListBaseHttpResponse<T> : IListBaseHttpResponse<T>
    {
        public List<T> Result { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }
    }

    public class BaseHttpResponse<T> : IBaseHttpResponse<T>
    {
        public T Result { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }
    }
}
