using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace RancherNodeAutoScaler.Rancher
{
    public class Node
    {
        [JsonProperty("actions")]
        public Dictionary<string, string> actions { get; set; }

        [JsonProperty("allocatable")]
        public Dictionary<string, string> Allocatable { get; set; }

        [JsonProperty("annotations")]
        public Dictionary<string, string> annotations { get; set; }

        [JsonProperty("baseType")]
        public string baseType { get; set; }

        [JsonProperty("capacity")]
        public Dictionary<string, string> Capacity { get; set; }

        [JsonProperty("clusterId")]
        public string clusterId { get; set; }

        [JsonProperty("conditions")]
        public List<object> conditions { get; set; }

        [JsonProperty("controlPlane")]
        public bool ControlPlane { get; set; }

        [JsonProperty("etcd")]
        public bool Etcd { get; set; }

        [JsonProperty("externalIpAddress")]
        public string ExternalIpAddress { get; set; }

        [JsonProperty("hostname")]
        public string hostName { get; set; }

        [JsonProperty("id")]
        public string id { get; set; }

        [JsonProperty("imported")]
        public bool imported { get; set; }
        
        [JsonProperty("ipAddress")]
        public string IpAddress { get; set; } 
        
        [JsonProperty("nodeName")]
        public string NodeName { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("state")]
        public string state { get; set; }
        
        [JsonProperty("worker")]
        public bool worker { get; set; }
    }
}
