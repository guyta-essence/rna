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
    public static class GetDataPoint
    {
        public static DataPoint GetAPoint(Cluster cluster)
        {
            DataPoint dataPoint = new DataPoint();
            GetClsuterMetrics(cluster);
            GetNodes(cluster);
            CalculateUsage(cluster);
            foreach (var item in cluster.Servers)
            {
                dataPoint.CpuUsage += item.CpuPercent;
            }
            dataPoint.CpuUsage = dataPoint.CpuUsage / cluster.Nodes.Count;
            if (dataPoint.CpuUsage > MySettings.highcpu)
                cluster.StressCount++;
            else
                cluster.StressCount = 0;

            if (dataPoint.CpuUsage < MySettings.lowcpu)
                cluster.LowStressCount++;
            else
                cluster.LowStressCount = 0;

            if (dataPoint.CpuUsage > MySettings.veryhighcpu)
                cluster.VeryHighStressCount++;
            else
                cluster.VeryHighStressCount = 0;

            return dataPoint;
        }

        private static void CalculateUsage(Cluster item)
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
        private static void GetNodes(Cluster item)
        {
            //Console.WriteLine("GetNodes " + DateTime.Now);
            string metricurl = MySettings.nodesurl.Replace("REPLACEME", item.Id);
            var json = GetJsonData.GetJson(metricurl, MySettings.token);
            var nodes = json.ToObject<List<Node>>();
            item.Nodes = nodes.Where(np => np.worker).ToList();
        }
        private static void GetClsuterMetrics(Cluster item)
        {
            //Console.WriteLine("GetClsuterMetrics " + DateTime.Now);
            string metricurl = MySettings.metricpath.Replace("REPLACEME", item.Id);
            var json = GetJsonData.GetJson(metricurl, MySettings.token);
            item.Servers = json["items"].ToObject<List<Server>>();
        }
    }
}
