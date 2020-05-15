using System;
using System.Collections.Generic;
using System.Text;

namespace RancherNodeAutoScaler.Metrics
{
    public class DataPoint
    {
        public int PodsCapacity { get; set; }
        public int PodsAlocated { get; set; }
        public Double PodsAlocatedPercent { get; set; }
        public int PodsPending { get; set; }

        public int MemCapacity { get; set; }
        public int MemAlocated { get; set; }
        public Double MemAlocatedPercent { get; set; }
        public int MemReserved { get; set; }
        public Double MemReservedPercent { get; set; }

        public int CpuCapacity { get; set; }
        public int CpuAlocated { get; set; }
        public Double CpuAlocatedPercent { get; set; }
        public int CpuReserverd { get; set; }
        public Double CpuReservedPercent { get; set; }

        public Double CpuUsage { get; set; }
        public DateTime dateTime { get; set; } = DateTime.Now;

    }
}
