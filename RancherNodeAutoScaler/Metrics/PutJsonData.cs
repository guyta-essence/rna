using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace RancherNodeAutoScaler.Metrics
{
    public static class PutJsonData
    {
        public static string PutJson(string url, string token, string data)
        {
            using (var httpClientHandler = new HttpClientHandler())
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                using (var client = new HttpClient(httpClientHandler))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var stringContent = new StringContent(data, Encoding.UTF8, "application/json");
                    var response = client.PutAsync(url, stringContent).Result;
                    return response.StatusCode + ":" + response.Content;
                }
            }
        }
    }
}
