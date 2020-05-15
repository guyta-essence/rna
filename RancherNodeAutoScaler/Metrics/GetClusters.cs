using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RancherMySettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace RancherNodeAutoScaler.Metrics
{
    class GetClusters
    {
        public List<Rancher.Cluster> GetClusterList()
        {
            using (var httpClientHandler = new HttpClientHandler())
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                using (var client = new HttpClient(httpClientHandler))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", MySettings.token);
                    var response = client.GetAsync(MySettings.nodepoolsurl).Result;
                    if (!response.IsSuccessStatusCode) return new List<Rancher.Cluster>();
                    var jsonString = response.Content.ReadAsStringAsync().Result;
                    var json = JsonConvert.DeserializeObject<JObject>(jsonString);
                    var clusters = json["data"].ToObject<List<Rancher.Cluster>>();
                    return clusters.Where(np => np.Name != "Local").ToList();
                }
            }
        }
    }
}
