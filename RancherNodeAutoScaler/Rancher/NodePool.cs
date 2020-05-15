using Newtonsoft.Json;
using RancherNodeAutoScaler.Metrics;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace RancherNodeAutoScaler.Rancher
{
    public class NodePool
    {

        [JsonProperty("annotations")]
        public Dictionary<string, string> annotations { get; set; }
        
        [JsonProperty("baseType")]
        public string baseType { get; set; }

        [JsonProperty("clusterId")]
        public string clusterId { get; set; }

        [JsonProperty("controlPlane")]
        public bool controlPlane { get; set; }

        [JsonProperty("created")]
        public string created { get; set; }

        [JsonProperty("createdTS")]
        public long createdTS { get; set; }

        [JsonProperty("creatorId")]
        public string creatorId { get; set; }

        [JsonProperty("deleteNotReadyAfterSecs")]
        public int deleteNotReadyAfterSecs { get; set; }

        [JsonProperty("displayName", NullValueHandling = NullValueHandling.Ignore)]
        public string displayName { get; set; }

        [JsonProperty("driver")]
        public string driver { get; set; }

        [JsonProperty("etcd")]
        public bool etcd { get; set; }

        [JsonProperty("hostnamePrefix")]
        public string hostnamePrefix { get; set; }

        [JsonProperty("id")]
        public string id { get; set; }

        [JsonProperty("labels")]
        public Dictionary<string, string> labels { get; set; }

        [JsonProperty("links")]
        public Dictionary<string, string> links { get; set; }

        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("namespaceId", NullValueHandling = NullValueHandling.Ignore)]
        public string namespaceId { get; set; }

        [JsonProperty("nodeAnnotations", NullValueHandling = NullValueHandling.Ignore)]
        public string nodeAnnotations { get; set; }

        [JsonProperty("nodeLabels", NullValueHandling = NullValueHandling.Ignore)]
        public string nodeLabels { get; set; }

        [JsonProperty("nodeTemplateId")]
        public string nodeTemplateId { get; set; }

        [JsonProperty("quantity")]
        public int quantity { get; set; }

        [JsonProperty("state")]
        public string state { get; set; }

        [JsonProperty("status")]
        public NodeStatus status { get; set; }

        [JsonProperty("transitioning")]
        public string transitioning { get; set; }

        [JsonProperty("transitioningMessage")]
        public string transitioningMessage { get; set; }

        [JsonProperty("type")]
        public string type { get; set; }

        [JsonProperty("uuid")]
        public string uuid { get; set; }

        [JsonProperty("worker")]
        public bool worker { get; set; }


        [JsonIgnore]
        public List<Server> Servers { get; set; }
        [JsonIgnore]
        public List<Node> Nodes { get; set; }
        [JsonIgnore] 
        public List<DataPoint> dataPoints { get; set; } = new List<DataPoint>();
        [JsonIgnore] 
        public int HighStressCount { get; set; } = 0;
        [JsonIgnore]
        public int LowStressCount { get; set; } = 0;
        [JsonIgnore] 
        public int VeryHighStressCount { get; set; } = 0;
        [JsonIgnore]
        public int activequantity { get; set; }
    }
    public class NodeStatus
    {
        [JsonProperty("conditions")]
        public List<object>conditions { get; set; }

        [JsonProperty("type")]
        public string type { get; set; }
    }
}
