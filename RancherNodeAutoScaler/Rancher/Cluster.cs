using Newtonsoft.Json;
using RancherNodeAutoScaler.Metrics;
using System;
using System.Collections.Generic;
using System.Text;

namespace RancherNodeAutoScaler.Rancher
{
    public class Cluster
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("allocatable")]
        public Resources Allocatable { get; set; }

        [JsonProperty("capacity")]
        public Resources Capacity { get; set; }

        [JsonProperty("requested")]
        public Resources Requested { get; set; }

        [JsonProperty("servers")]
        public List<Server> Servers { get; set; }

        [JsonProperty("nodes")]
        public List<Node> Nodes { get; set; }

        [JsonIgnore]
        public List<DataPoint> dataPoints { get; set; } = new List<DataPoint>();
        [JsonIgnore]
        public int StressCount { get; set; } = 0;
        [JsonIgnore]
        public int VeryHighStressCount { get; set; } = 0;
        [JsonIgnore]
        public int LowStressCount { get; set; } = 0;
        [JsonIgnore]
        public bool IsAddingorRemovingNode { get; set; } = false;

    }
}
