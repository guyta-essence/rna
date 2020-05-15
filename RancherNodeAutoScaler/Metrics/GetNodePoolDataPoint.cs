using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RancherMySettings;
using RancherNodeAutoScaler.Rancher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace RancherNodeAutoScaler.Metrics
{
    public class GetNodePoolDataPoint
    {
        public static DataPoint GetAPoint(NodePool nodePool)
        {
            DataPoint dataPoint = new DataPoint();
            GetClsuterMetrics(nodePool);
            GetNodes(nodePool);
            CalculateUsage(nodePool);
            foreach (var item in nodePool.Servers)
            {
                dataPoint.CpuUsage += item.CpuPercent;
            }
            dataPoint.CpuUsage = dataPoint.CpuUsage / nodePool.activequantity;
            if (dataPoint.CpuUsage > MySettings.highcpu)
                nodePool.HighStressCount++;
            else
                nodePool.HighStressCount = 0;

            if (dataPoint.CpuUsage < MySettings.lowcpu)
                nodePool.LowStressCount++;
            else
                nodePool.LowStressCount = 0;
            
            if (dataPoint.CpuUsage > MySettings.veryhighcpu)
                nodePool.VeryHighStressCount++;
            else
                nodePool.VeryHighStressCount = 0;

            return dataPoint;
        }

        private static void CalculateUsage(NodePool item)
        {
            foreach (var titem in item.Servers)
            {
                foreach (var bitem in item.Nodes)
                {
                    if (titem.Metadata["name"] == bitem.hostName)
                    {
                        double cpunano;
                        int cores = int.Parse(bitem.Capacity["cpu"]);
                        if (double.TryParse(titem.Usage["cpu"].Substring(0, titem.Usage["cpu"].Length - 1), out cpunano))
                        {
                            double percent = (cpunano / 1000000000) / cores;// * 1000 * cores;
                            //Console.WriteLine(bitem.hostName + "\t" + Math.Floor(percent * 100) + "%");
                            titem.CpuPercent = Math.Floor(percent * 100);
                        }
                    }
                }
            }
        }
        public static void GetNodes(NodePool item)
        {
            string metricurl = MySettings.nodesurl.Replace("REPLACEME", item.clusterId);
            var json = GetJsonData.GetJson(metricurl, MySettings.token);
            var nodes = json["data"].ToObject<List<Node>>();
            item.Nodes = nodes.Where(np => np.worker).ToList();
            item.quantity = item.Nodes.Count;
            item.activequantity = 0;
            foreach (var active in item.Nodes)
                if (active.state == "active")
                    item.activequantity++;
        }
        private static void GetClsuterMetrics(NodePool item)
        {
            string metricurl = MySettings.metricpath.Replace("REPLACEME", item.clusterId);
            var json = GetJsonData.GetJson(metricurl, MySettings.token);
            item.Servers = json["items"].ToObject<List<Server>>();
        }
    }
}
