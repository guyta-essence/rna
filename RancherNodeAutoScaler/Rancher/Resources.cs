using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace RancherNodeAutoScaler.Rancher
{
    public class Resources
    {
        [JsonProperty("cpu")]
        public string Cpu { get; set; }

        [JsonProperty("memory")]
        public string Memory { get; set; }

        [JsonProperty("pods")]
        public string Pods { get; set; }
    }
}
