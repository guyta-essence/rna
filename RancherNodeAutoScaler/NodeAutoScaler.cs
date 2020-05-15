using Microsoft.Extensions.Hosting;
using RancherNodeAutoScaler.Metrics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using RancherNodeAutoScaler.Rancher;
using System.Linq;
using RancherMySettings;

namespace RancherNodeAutoScaler
{
    public class NodeAutoScaler : IHostedService, IDisposable
    {
        private readonly System.Timers.Timer _timer = new System.Timers.Timer();
        List<NodePool> nodePools;

        public void Dispose()
        {
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                MySettings.DefaultSettings();

                _timer.Interval = MySettings.SampleRate * 1000;
                _timer.Elapsed += TimerMethod;
                _timer.AutoReset = false;
                _timer.Start();

                var json = GetJsonData.GetJson(MySettings.nodepoolsurl, MySettings.token);
                var Pools = json["data"].ToObject<List<Rancher.NodePool>>();
                nodePools = Pools.Where(np => np.worker).ToList();
                if (nodePools.Count == 0)
                {
                    Console.WriteLine("Node pools empty!");
                    return StopAsync(cancellationToken);
                }

                Console.WriteLine($"[{nameof(NodeAutoScaler)}] has been started.....");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Task.CompletedTask;
        }

        private void TimerMethod(object sender, ElapsedEventArgs args)
        {
            Console.WriteLine("*** TimerMethod is executed at {0} ***", DateTime.Now);
            try
            {
                CollectData();
                CheckStress();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            //Console.WriteLine("dataPoints.datapoints.Count:" + dataPoints.datapoints.Count);
            _timer.Start();
        }

        private void CheckStress()
        {
            Console.WriteLine("SampleTriggerCount " + MySettings.SampleTriggerCount + ", VeryHighSampleTriggerCount " + MySettings.VeryHighSampleTriggerCount + ", maximumnodes " + MySettings.maximumnodes + ", minimumnodes " + MySettings.minimumnodes);
            Console.WriteLine("{0,-20}{1,-20}{2,-20}", "Nodepool Prefix", "quantity","active nodes");
            foreach (var nodepool in nodePools)
            {
                Console.WriteLine("{0,-20}{1,-20}{2,-20}", nodepool.hostnamePrefix, nodepool.quantity, nodepool.activequantity);
                if (nodepool.HighStressCount > MySettings.SampleTriggerCount && nodepool.quantity < MySettings.maximumnodes)
                {
                    AddRemoveNode.AddNode(nodepool);
                    Console.WriteLine(nodepool.hostnamePrefix + " adding node " + nodepool.HighStressCount);
                    nodepool.HighStressCount = 0;
                }
                if (nodepool.VeryHighStressCount > MySettings.VeryHighSampleTriggerCount && nodepool.quantity < MySettings.maximumnodes)
                {
                    AddRemoveNode.AddNode(nodepool);
                    Console.WriteLine(nodepool.hostnamePrefix + " adding node " + nodepool.VeryHighStressCount);
                    nodepool.VeryHighStressCount = 0;
                    nodepool.HighStressCount = 0;
                }
                if (nodepool.LowStressCount > MySettings.SampleTriggerCount && nodepool.quantity > MySettings.minimumnodes)
                {
                    AddRemoveNode.RemoveNode(nodepool);
                    Console.WriteLine(nodepool.hostnamePrefix + " removeing node " + nodepool.LowStressCount);
                    nodepool.LowStressCount = 0;
                }
            }
        }

        private void CollectData()
        {
            Console.WriteLine("{0,-20}{1,-20}{2,-25}{3,-20}{4,-20}", "Nodepool prefix", "High Stress Count", "Very High Stress Count", "Low Stress Count", "Last Value%");
            foreach (var nodepool in nodePools)
            {
                if (nodepool.dataPoints.Count > MySettings.NumberofSamples)
                    nodepool.dataPoints.RemoveAt(0);
                nodepool.dataPoints.Add(GetNodePoolDataPoint.GetAPoint(nodepool));
                Console.WriteLine("{0,-20}{1,-20}{2,-25}{3,-20}{4,-20}", nodepool.hostnamePrefix, nodepool.HighStressCount, nodepool.VeryHighStressCount, nodepool.LowStressCount, Math.Floor(nodepool.dataPoints.Last().CpuUsage) + "%");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine($"[{nameof(NodeAutoScaler)}] has been stopped.....");
            Thread.Sleep(1000);
            return Task.CompletedTask;
        }
    }
}
