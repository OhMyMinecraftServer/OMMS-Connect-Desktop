using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OMMSClientCoreCSharp.Utils;


namespace OMMSClientCoreCSharp
{
    namespace Utils
    {
        public class Util
        {

            #region Constants
            public static readonly long version = 0xc0000000L + 1; //Central ~<=0.7

            #endregion

            #region Json
            public static String ToJson(object obj)
            {
                return JsonConvert.SerializeObject(obj, Formatting.None);
            }

            public static T FromJson<T>(String json)
            {
                return JsonConvert.DeserializeObject<T>(json);
            }


            #endregion

            #region Base64

            public static String Base64Encode(String src)
            {
                return Convert.ToBase64String(Encoding.UTF8.GetBytes(src));
            }

            public static String Base64Decode(String b64)
            {
                return Encoding.Unicode.GetString(Convert.FromBase64String(b64));
            }

            #endregion
        }
    }

    namespace Data
    {
        #region Controller

        public class Controller
        {
            public string name { get; set; }
            public string executable { get; set; }
            public string type { get; set; }
            public string launchParams { get; set; }
            public string workingDir { get; set; }
            public bool statusQueryable { get; set; }
        }

        public class ControllerInstance
        {
            public Controller controller { get; set; }
            public string controllerType { get; set; }

        }

        #endregion

        #region Announcement

        public class Announcement
        {
            public string id { get; set; }
            public int timeMillis { get; set; }
            public string title { get; set; }
            public List<string> content { get; set; }
        }

        #endregion

        #region Whitelist

        public class Whitelist
        {
            public List<string> players { get; set; }
            public string name { get; set; }
        }

        #endregion

        #region System

        public class Filesystem
        {
            public int free { get; set; }
            public int total { get; set; }
            public string volume { get; set; }
            public string mountPoint { get; set; }
            public string fileSystemType { get; set; }
        }

        public class FileSystemInfo
        {
            public List<Filesystem> filesystems { get; set; }
        }

        public class MemoryInfo
        {
            public int memoryTotal { get; set; }
            public int memoryUsed { get; set; }
            public int swapTotal { get; set; }
            public int swapUsed { get; set; }
        }

        public class NetworkInterface
        {
            public string name { get; set; }
            public string displayName { get; set; }
            public string macAddress { get; set; }
            public int mtu { get; set; }
            public int speed { get; set; }
            public List<string> ipv4Address { get; set; }
            public List<string> ipv6Address { get; set; }
        }

        public class NetworkInfo
        {
            public List<NetworkInterface> networkInterfaceList { get; set; }
            public string hostName { get; set; }
            public string domainName { get; set; }
            public List<string> dnsServers { get; set; }
            public string ipv4DefaultGateway { get; set; }
            public string ipv6DefaultGateway { get; set; }
        }

        public class ProcessorInfo
        {
            public int physicalCPUCount { get; set; }
            public int logicalProcessorCount { get; set; }
            public string processorName { get; set; }
            public int cpuLoadAvg { get; set; }
            public string processorId { get; set; }
            public int cpuTemp { get; set; }
        }

        public class StorageItem
        {
            public string name { get; set; }
            public string model { get; set; }
            public int size { get; set; }
        }

        public class StorageInfo
        {
            public List<StorageItem> storages { get; set; }
        }

        public class SystemInfo
        {
            public string osName { get; set; }
            public string osVersion { get; set; }
            public string osArch { get; set; }
            public FileSystemInfo fileSystemInfo { get; set; }
            public MemoryInfo memoryInfo { get; set; }
            public NetworkInfo networkInfo { get; set; }
            public ProcessorInfo processorInfo { get; set; }
            public StorageInfo storageInfo { get; set; }
        }


        #endregion

        #region Session

        #region Request
        public class Request
        {
            public string request { get; set; }
            public Dictionary<String, String> content { get; set; }

            public Request WithContentKeyPair(String k, String v)
            {
                content.Add(k, v);
                return this;
            }

            public Request(String request)
            {
                this.request = request;
                content = new Dictionary<string, string>();
            }

            public Request()
            {
                this.request = "";
                content = new Dictionary<string, string>();
            }

            public override string ToString()
            {
                return base.ToString();
            }
        }

        public class InitRequest : Request
        {
            public long version { get; set; }

            public static readonly long VERSION_BASE = 0xc000_0000L;

            public InitRequest(string request, long version) : base(request)
            {
                this.version = version;
            }

            public InitRequest(Request request, long version) : base()
            {
                this.request = request.request;
                this.content = request.content;
                this.version = version;
            }
        }
        #endregion

        #region Response

        public class Response
        {
            public String code { get; set; }
            public Dictionary<String, String> content { get; }
            public static String serialize(Response response)
            {
                return Util.ToJson(response);
            }

            public Response(string code, Dictionary<string, string> content)
            {
                this.code = code;
                this.content = content;
            }

            public Response(string code)
            {
                this.content = new Dictionary<String, String>();
                this.code = code;
            }

            public static Response deserialize(String x)
            {
                return Util.FromJson<Response>(x);
            }

            public Response withResponseCode(String code)
            {
                this.code = code;
                return this;
            }

            public Response withContentPair(String a, String b)
            {
                content.Add(a, b);
                return this;
            }

            public String getContent(String key)
            {
                String value;
                if (!content.TryGetValue(key, out value))
                {
                    return null;
                }

                return value;
            }

        }
        

        #endregion



        #endregion
    }
}