using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static System.Net.Http.HttpMethod;

namespace sppenyakitlambung.Utilities.Helper
{
    public static class HttpServiceHelper
    {
        public static async Task<IHttpResponse> ProcessHttpRequestAsync<TRequest, TResponse>(HttpMethod httpMethod, string url, TRequest request, string accessToken = null, string tokenName = "Authorization")
        {
            using (var httpClient = new HttpClient())
            {

                IHttpResponse response = null;
                HttpResponseMessage httpResponse = null;

                if (accessToken != null)
                {
                    httpClient.DefaultRequestHeaders.Add(tokenName, accessToken);
                }

                try
                {
                    var content = new ByteArrayContent(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(request)));
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    switch (httpMethod)
                    {
                        case HttpMethod method when method == Get: { httpResponse = await httpClient.GetAsync(url); break; }
                        case HttpMethod method when method == Post: { httpResponse = await httpClient.PostAsync(url, content); break; }
                        case HttpMethod method when method == Put: { httpResponse = await httpClient.PutAsync(url, content); break; }
                        case HttpMethod method when method == Delete: { httpResponse = await httpClient.DeleteAsync(url); break; }
                        default: { throw new ArgumentOutOfRangeException(nameof(httpMethod)); }
                    }

                    if (httpResponse.IsSuccessStatusCode)
                    {
                        var jsonString = await httpResponse.Content.ReadAsStringAsync();
                        var anObject = JsonConvert.DeserializeObject<object>(jsonString);
                        switch (anObject)
                        {
                            case JArray jsonObjectArray:
                                {
                                    response = new ListBaseHttpResponse<TResponse>
                                    {
                                        Result = jsonObjectArray.ToObject<List<TResponse>>(),
                                        StatusCode = httpResponse.StatusCode,
                                        Message = "Success"
                                    };
                                    break;
                                }
                            case JObject jsonObject:
                                {
                                    response = new BaseHttpResponse<TResponse>
                                    {
                                        Result = jsonObject.ToObject<TResponse>(),
                                        StatusCode = httpResponse.StatusCode,
                                        Message = "Success"
                                    };
                                    break;
                                }
                        }
                    }
                    else
                    {
                        response = new BaseHttpResponse<TResponse>
                        {
                            StatusCode = httpResponse.StatusCode,
                            Result = default(TResponse),
                            Message = await httpResponse.Content.ReadAsStringAsync()
                        };
                    }
                }
                catch (Exception exception)
                {
                    response = new BaseHttpResponse<TResponse>
                    {
                        StatusCode = httpResponse?.StatusCode ?? HttpStatusCode.InternalServerError,
                        Result = default(TResponse),
                        Message = exception.Message
                    };
                }
                return response;
            }
        }

        public static async Task<IHttpResponse> ProcessHttpRequestAsync<T>(HttpMethod httpMethod, string url, T model, string accessToken = null)
        {
            using (var httpClient = new HttpClient())
            {
                IHttpResponse response = null;
                HttpResponseMessage httpResponse = null;

                if (accessToken != null)
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", accessToken);
                }

                try
                {
                    var content = new ByteArrayContent(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(model)));
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    switch (httpMethod)
                    {
                        case HttpMethod method when method == Get: { httpResponse = await httpClient.GetAsync(url); break; }
                        case HttpMethod method when method == Post: { httpResponse = await httpClient.PostAsync(url, content); break; }
                        case HttpMethod method when method == Put: { httpResponse = await httpClient.PutAsync(url, content); break; }
                        case HttpMethod method when method == Delete: { httpResponse = await httpClient.DeleteAsync(url); break; }
                        default: { throw new ArgumentOutOfRangeException(nameof(httpMethod)); }
                    }

                    if (httpResponse.IsSuccessStatusCode)
                    {
                        string jsonString = await httpResponse.Content.ReadAsStringAsync();
                        var anObject = JsonConvert.DeserializeObject<object>(jsonString);
                        switch (anObject)
                        {
                            case JArray jsonObjectArray:
                                {
                                    response = new ListBaseHttpResponse<T>
                                    {
                                        Result = jsonObjectArray.ToObject<List<T>>(),
                                        StatusCode = httpResponse.StatusCode,
                                        Message = "Success"
                                    };
                                    break;
                                }
                            case JObject jsonObject:
                                {
                                    response = new BaseHttpResponse<T>
                                    {
                                        Result = jsonObject.ToObject<T>(),
                                        StatusCode = httpResponse.StatusCode,
                                        Message = "Success"
                                    };
                                    break;
                                }
                        }
                    }
                    else
                    {
                        response = new BaseHttpResponse<T>
                        {
                            StatusCode = httpResponse.StatusCode,
                            Result = default(T),
                            Message = await httpResponse.Content.ReadAsStringAsync()
                        };
                    }
                }
                catch (Exception exception)
                {
                    response = new BaseHttpResponse<T>
                    {
                        StatusCode = httpResponse?.StatusCode ?? HttpStatusCode.InternalServerError,
                        Result = default(T),
                        Message = exception.Message
                    };
                }
                return response;
            }
        }
    }
}
