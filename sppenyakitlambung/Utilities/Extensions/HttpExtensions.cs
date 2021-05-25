using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using sppenyakitlambung.Services;

namespace sppenyakitlambung.Extensions
{
    public static class HttpExtensions
    {
        public static void ConfigureAuthorization(this HttpClient httpClient, string authorizationToken = null, string authorizationScheme = "Bearer")
        {
            if (!string.IsNullOrWhiteSpace(authorizationToken) && !string.IsNullOrWhiteSpace(authorizationScheme))
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(authorizationScheme, authorizationToken);
            }
            else
            {
                httpClient.DefaultRequestHeaders.Authorization = null;
            }
        }

        public static async Task<string> GetContent(this HttpResponseMessage httpResponseMessage, string url, bool enableHttpResponseLog = false)
        {
            string httpResponseContent = await httpResponseMessage.Content.ReadAsStringAsync();

            if (httpResponseMessage.IsSuccessStatusCode || httpResponseMessage.StatusCode == HttpStatusCode.NotFound)
            {
                return httpResponseContent;
            }
            else
            {
                if (enableHttpResponseLog)
                {
                    string error = $"Http request unsuccessful!{Environment.NewLine}";
                    error += $"Status Code: {(int)httpResponseMessage.StatusCode}{Environment.NewLine}";
                    error += $"Reason: {httpResponseMessage.ReasonPhrase}{Environment.NewLine}";
                    error += $"Url: {url}{Environment.NewLine}";

                    foreach (var authenticationHeaderValue in httpResponseMessage.Headers.WwwAuthenticate)
                    {
                        error += $"Scheme: {authenticationHeaderValue.Scheme}{Environment.NewLine}";

                        if (authenticationHeaderValue.Parameter != null)
                        {
                            error += $"Parameter: {authenticationHeaderValue.Parameter}{Environment.NewLine}";

                            if (authenticationHeaderValue.Parameter.Contains("invalid_token"))
                            {
                                error += $"The authorization token is invalid or expired!{Environment.NewLine}";
                            }
                        }
                    }

                    error += $"HttpResponseContent: {httpResponseContent}{Environment.NewLine}";
                    LoggingService.LogErrorMessage(null, error);
                }
            }

            return httpResponseContent;
        }
    }
}
