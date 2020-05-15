using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RancherMySettings;
using RancherNodeAutoScaler.Metrics;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Text.Unicode;

namespace RancherNodeAutoScaler.Rancher
{
    public static class AddRemoveNode
    {
        public static void AddNode(NodePool nodePool)
        {
            var nodepollurl = MySettings.nodepoolsurl + nodePool.id;
            var json = GetJsonData.GetJson(nodepollurl, MySettings.token);
            var q = json["quantity"];
            json["quantity"] = (int)q + 1;
            var resault = PutJsonData.PutJson(nodepollurl, MySettings.token, json.ToString());
            Console.WriteLine("AddNode " + resault);
        }
        public static void RemoveNode(NodePool nodePool)
        {
            foreach (var node in nodePool.Nodes)
            {
                if(node.hostName == nodePool.hostnamePrefix + nodePool.quantity.ToString())
                {
                    Console.WriteLine("DrainNode " + node.hostName);
                    DrainNode(node);
                }
            }
            var nodepollurl = MySettings.nodepoolsurl + nodePool.id;
            var json = GetJsonData.GetJson(nodepollurl, MySettings.token);
            var q = json["quantity"];
            json["quantity"] = (int)q - 1;
            var resault = PutJsonData.PutJson(nodepollurl, MySettings.token, json.ToString());
            Console.WriteLine("RemoveNode " + resault);
        }

        private static void DrainNode(Node node)
        {
            string json = "{\r\n    \"deleteLocalData\": false,\r\n    \"force\": true,\r\n    \"gracePeriod\": -1,\r\n    \"ignoreDaemonSets\": true,\r\n    \"timeout\": 60\r\n}";
            var response = PutJsonData.PutJson(node.actions["drain"], MySettings.token, json);
            Console.WriteLine(response);
        }
    }
}
