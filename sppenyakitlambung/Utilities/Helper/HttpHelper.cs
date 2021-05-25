using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace sppenyakitlambung.Helper
{
    public static class HttpHelper
    {
        public static ByteArrayContent ConvertToByteArrayContent<T>(T obj)
        {
            string jsonItem = JsonConvert.SerializeObject(obj);
            byte[] bytesJsonItem = Encoding.UTF8.GetBytes(jsonItem);
            var byteArrayContent = new ByteArrayContent(bytesJsonItem);
            byteArrayContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return byteArrayContent;
        }
    }
}
