using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMMS.Core.Models;

public class SystemInfo
{
    [JsonProperty("osName")]
    public string OperationSystem { get; set; }

    [JsonProperty("osVersion")]
    public string OperationSystemVersion { get; set; }

    [JsonProperty("osArch")]
    public string SystemArch { get; set; }

    [JsonProperty("fileSystemInfo")]
    public FileSystemInfo FileSystemInfomation { get; set; }

    [JsonProperty("storageInfo")]
    public DriverInfo DriverInfomation { get; set; }

    [JsonProperty("processorInfo")]
    public ProcessorInfo ProcessorInfomation { get; set; }

    [JsonProperty("memoryInfo")]
    public MemoryInfo MemoryInfomation { get; set; }

    [JsonProperty("networkInfo")]
    public NetworkInfo NetworkInfomation { get; set; }

    public class FileSystemInfo
    {
        [JsonProperty("fileSystems")]
        public IEnumerable<DiskInfo> Disks { get; set; }

        public class DiskInfo
        {
            [JsonProperty("free")]
            public long Free { get; set; }

            [JsonProperty("total")]
            public long Total { get; set; }

            [JsonProperty("volume")]
            public string Volume { get; set; }

            [JsonProperty("mountPoint")]
            public string MountPoint { get; set; }

            [JsonProperty("fileSystemType")]
            public string FileSystem { get; set; }
        }
    }

    public class DriverInfo
    {
        [JsonProperty("storages")]
        public IEnumerable<Driver> Drivers { get; set; }

        public class Driver
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("model")]
            public string DisplayName { get; set; }

            [JsonProperty("size")]
            public long Size { get; set; }
        }
    }

    public class ProcessorInfo
    {
        [JsonProperty("physicalCPUCount")]
        public int PhysicalCoreCount { get; set; }

        [JsonProperty("logicalCPUCount")]
        public int LogicalCoreCount { get; set; }

        [JsonProperty("processorName")]
        public string ProcessorName { get; set; }

        [JsonProperty("processorId")]
        public string ProcessorId { get; set; }

        [JsonProperty("cpuTemp")]
        public double CpuTemp { get; set; }

        [JsonProperty("cpuLoadAvg")]
        public double CpuLoadAvg { get; set; }
    }

    public class MemoryInfo
    {
        [JsonProperty("memoryUsed")]
        public long MemoryUsed { get; set; }

        [JsonProperty("memoryTotal")]
        public long MemoryTotal { get; set; }

        [JsonProperty("swapUsed")]
        public long SwapUsed { get; set; }

        [JsonProperty("swapTotal")]
        public long SwapTotal { get; set; }
    }

    public class NetworkInfo
    {
        [JsonProperty("networkInterfaceList")]
        public IEnumerable<NetworkInterface> NetworkInterfaces { get; set; }

        public class NetworkInterface
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("displayName")]
            public string DisplayName { get; set; }

            [JsonProperty("macAddress")]
            public string Mac { get; set; }

            [JsonProperty("mtu")]
            public string Mtu { get; set; }

            //[JsonProperty("speed")]
            //public string speed { get; set; }

            [JsonProperty("ipv4Address")]
            public IEnumerable<string> Ipv4Address { get; set; }

            [JsonProperty("ipv6Address")]
            public IEnumerable<string> Ipv6Address { get; set; }
        }

        [JsonProperty("hostName")]
        public string HostName { get; set; }

        [JsonProperty("domainName")]
        public string DomainName { get; set; }
            
        [JsonProperty("ipv4DefaultGateway")]
        public string Ipv4DefaultGateway { get; set; }

        [JsonProperty("ipv6DefaultGateway")]
        public string Ipv6DefaultGateway { get; set; }

        [JsonProperty("dnsServers")]
        public IEnumerable<string> DnsServers { get; set; }
    }
}
