using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using sppenyakitlambung.Extensions;
using sppenyakitlambung.Helper;
using sppenyakitlambung.Models;

namespace sppenyakitlambung.Services
{
    public static class HttpService
    {
        public async static Task<HttpRequest<T>> DeleteAsync<T>(string url, T obj, string authorizationToken = null, string authorizationScheme = "Bearer", bool enableHttpResponseLog = true, HttpClient httpClient = null)
        {
            return await ProcessRequestAsync<T, T>("Delete", url, obj, authorizationToken, authorizationScheme, enableHttpResponseLog, httpClient);
        }

        public async static Task<HttpRequest<TResult>> DeleteAsync<T, TResult>(string url, T obj, string authorizationToken = null, string authorizationScheme = "Bearer", bool enableHttpResponseLog = true, HttpClient httpClient = null)
        {
            return await ProcessRequestAsync<T, TResult>("DeleteWithReturn", url, obj, authorizationToken, authorizationScheme, enableHttpResponseLog, httpClient);
        }

        public async static Task<HttpRequest<T>> GetAsync<T>(string url, string authorizationToken = null, string authorizationScheme = "Bearer", bool enableHttpResponseLog = true, HttpClient httpClient = null)
        {
            return await ProcessRequestAsync<T, T>("Get", url, default, authorizationToken, authorizationScheme, enableHttpResponseLog, httpClient);
        }

        public async static Task<HttpRequest<T>> PostAsync<T>(string url, T obj, string authorizationToken = null, string authorizationScheme = "Bearer", bool enableHttpResponseLog = true, HttpClient httpClient = null)
        {
            return await ProcessRequestAsync<T, T>("Post", url, obj, authorizationToken, authorizationScheme, enableHttpResponseLog, httpClient);
        }

        public async static Task<HttpRequest<TResult>> PostAsync<T, TResult>(string url, T obj, string authorizationToken = null, string authorizationScheme = "Bearer", bool enableHttpResponseLog = true, HttpClient httpClient = null)
        {
            return await ProcessRequestAsync<T, TResult>("PostWithReturn", url, obj, authorizationToken, authorizationScheme, enableHttpResponseLog, httpClient);
        }

        public async static Task<HttpRequest<T>> PutAsync<T>(string url, T obj, string authorizationToken = null, string authorizationScheme = "Bearer", bool enableHttpResponseLog = true, HttpClient httpClient = null)
        {
            return await ProcessRequestAsync<T, T>("Put", url, obj, authorizationToken, authorizationScheme, enableHttpResponseLog, httpClient);
        }

        public async static Task<HttpRequest<TResult>> PutAsync<T, TResult>(string url, T obj, string authorizationToken = null, string authorizationScheme = "Bearer", bool enableHttpResponseLog = true, HttpClient httpClient = null)
        {
            return await ProcessRequestAsync<T, TResult>("PutWithReturn", url, obj, authorizationToken, authorizationScheme, enableHttpResponseLog, httpClient);
        }

        private async static Task<HttpRequest<TResult>> ProcessRequestAsync<T, TResult>(string httpMethod, string url, T obj = default, string authorizationToken = null, string authorizationScheme = "Bearer", bool enableHttpResponseLog = true, HttpClient httpClient = null)
        {
            if (httpClient == null)
            {
                if (HttpClient != null)
                {
                    // We are using the global client for http requests therefore for thread safety we need to
                    // wait and release the semaphore as only 1 thread at a time should use this publically static client.
                    await __SemaphoreSlim.WaitAsync();
                    httpClient = HttpClient;
                }
                else
                {
                    httpClient = new HttpClient();
                }
            }

            var httpRequest = new HttpRequest<TResult>(url);
            HttpResponseMessage httpResponseMessage = null;

            try
            {
                httpClient.ConfigureAuthorization(authorizationToken, authorizationScheme);

                switch (httpMethod)
                {
                    case "Delete":
                    case "DeleteWithReturn":
                        httpResponseMessage = await httpClient.DeleteAsync(url);
                        break;

                    case "Get":
                        httpResponseMessage = await httpClient.GetAsync(url);
                        break;

                    case "Post":
                    case "PostWithReturn":
                        httpResponseMessage = await httpClient.PostAsync(url, HttpHelper.ConvertToByteArrayContent<T>(obj));
                        break;

                    case "Put":
                    case "PutWithReturn":
                        httpResponseMessage = await httpClient.PutAsync(url, HttpHelper.ConvertToByteArrayContent<T>(obj));
                        break;
                }

                httpRequest.HttpResponseContent = await httpResponseMessage.GetContent(url, enableHttpResponseLog);
                httpRequest.HttpStatusCode = httpResponseMessage.StatusCode;
                httpRequest.Successful = httpResponseMessage.IsSuccessStatusCode;

                if (httpRequest.Successful && (httpMethod == "Get" || httpMethod == "DeleteWithReturn" || httpMethod == "PostWithReturn" || httpMethod == "PutWithReturn"))
                {
                    SerializationHelper.DeserializeFromJsonString<TResult>(httpRequest.HttpResponseContent, out TResult resultObj);
                    httpRequest.Result = resultObj;
                }
            }
            catch (Exception exception)
            {
                httpRequest.HttpStatusCode = HttpStatusCode.InternalServerError;
                LoggingService.LogErrorMessage(exception, $"HttpService.ProcessRequestAsync<{typeof(T).FullName}>({httpMethod})");
            }
            finally
            {
                if (HttpClient != null)
                {
                    __SemaphoreSlim.Release();
                }
            }

            return httpRequest;
        }

        private static SemaphoreSlim __SemaphoreSlim = new SemaphoreSlim(1, 1);

        public static HttpClient HttpClient;
    }
}
