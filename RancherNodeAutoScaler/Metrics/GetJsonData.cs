using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace RancherNodeAutoScaler.Metrics
{
    public static class GetJsonData
    {
        public static JObject GetJson(string url, string token)
        {
            using (var httpClientHandler = new HttpClientHandler())
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                using (var client = new HttpClient(httpClientHandler))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var response = client.GetAsync(url).Result;
                    if (!response.IsSuccessStatusCode) return new JObject();
                    var jsonString = response.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<JObject>(jsonString);
                }
            }
        }
    }
}
