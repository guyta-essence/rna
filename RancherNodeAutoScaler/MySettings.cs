using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace RancherMySettings
{
    public static class MySettings
    {
        public static string rancherurl { get; set; }//if null or empty throws exception
        public static string token { get; set; }//if null or empty throws exception
        public static string clusterurl { get; set; }
        public static string metricpath { get; set; }
        public static string nodesurl { get; set; }
        public static string nodepoolsurl { get; set; }
        public static int minimumnodes { get; set; } = 3;
        public static int maximumnodes { get; set; } = 10;
        public static int NumberofSamples { get; set; } = 99;
        public static int highcpu { get; set; } = 75;
        public static int veryhighcpu { get; set; } = 95;
        public static int lowcpu { get; set; } = 20;
        public static int TriggerDuration { get; set; } = 5; //number of minutes to trigger update
        public static int VeryHighTriggerDuration { get; set; } = 2;//number of minutes to trigger updat
        public static int SampleRate { get; set; } = 15; //number of seconds betwwen samples

        //(60 seconds/seconds betwwen samples)* number of mintues to trriger chanage e.g. (60/15)*5=20 (20 samples of high cpu trigges add node
        public static int SampleTriggerCount { get; set; }
        public static int VeryHighSampleTriggerCount { get; set; }

        public static void DefaultSettings()
        {
            int iout;
            
            MySettings.rancherurl = Environment.GetEnvironmentVariable("RNA_rancherurl");
            if (MySettings.rancherurl == "" || MySettings.rancherurl == null)
                throw new NullReferenceException("Rancher url cannot be null, set enviorment variable 'RNA_rancherurl'");
            MySettings.token = Environment.GetEnvironmentVariable("RNA_ranchertoken");
            if (MySettings.token == "" || MySettings.token == null)
                throw new NullReferenceException("Rancher token cannot be null, set enviorment variable 'RNA_ranchertoken'");

            MySettings.clusterurl = MySettings.rancherurl + "/v3/clusters";
            MySettings.metricpath = MySettings.rancherurl + "/k8s/clusters/REPLACEME/apis/metrics.k8s.io/v1beta1/nodes";
            MySettings.nodesurl = MySettings.rancherurl + "/v3/clusters/REPLACEME/nodes";
            MySettings.nodepoolsurl = MySettings.rancherurl + "/v3/nodePools/";

            if (int.TryParse(Environment.GetEnvironmentVariable("RNA_minimumnodes"), out iout))
                MySettings.minimumnodes = iout;
            if (int.TryParse(Environment.GetEnvironmentVariable("RNA_maximumnodes"), out iout))
                MySettings.maximumnodes = iout;
            if (int.TryParse(Environment.GetEnvironmentVariable("RNA_NumberofSamples"), out iout))
                MySettings.NumberofSamples = iout;
            if (int.TryParse(Environment.GetEnvironmentVariable("RNA_highcpu"), out iout))
                MySettings.highcpu = iout;
            if (int.TryParse(Environment.GetEnvironmentVariable("RNA_veryhighcpu"), out iout))
                MySettings.veryhighcpu = iout;
            if (int.TryParse(Environment.GetEnvironmentVariable("RNA_lowcpu"), out iout))
                MySettings.lowcpu = iout;
            if (int.TryParse(Environment.GetEnvironmentVariable("RNA_TriggerDuration"), out iout))
                MySettings.TriggerDuration = iout;
            if (int.TryParse(Environment.GetEnvironmentVariable("RNA_VeryHighTriggerDuration"), out iout))
                MySettings.VeryHighTriggerDuration = iout;
            if (int.TryParse(Environment.GetEnvironmentVariable("RNA_SampleRate"), out iout))
                MySettings.SampleRate = iout;

            MySettings.SampleTriggerCount = (60 / MySettings.SampleRate) * MySettings.TriggerDuration;
            MySettings.VeryHighSampleTriggerCount = (60 / MySettings.SampleRate) * MySettings.VeryHighTriggerDuration;
        }
    }
}
