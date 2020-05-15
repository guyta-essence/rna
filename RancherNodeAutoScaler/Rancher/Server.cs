using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace RancherNodeAutoScaler.Rancher
{
    public class Server
    {
        [JsonProperty("metadata")]
        public Dictionary<string, string> Metadata { get; set; }

        [JsonProperty("usage")]
        public Dictionary<string, string> Usage { get; set; }

        [JsonProperty("cpupercent")]
        public Double CpuPercent { get; set; }
    }
}
