using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using OMMS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMMS.Core.Components;

public static class OMMSCentralClientExtension
{
    public static async Task<SystemInfo> GetSystemInfo(this OMMSCentralClient client)
        => JsonConvert.DeserializeObject<SystemInfo>((await client.SendAndGetReply<JObject>(new("SYSTEM_GET_INFO"))).Content["systemInfo"].ToString());

    public static async Task<IEnumerable<string>> GetControllers(this OMMSCentralClient client)
        => (await client.SendAndGetReply<JObject>(new("CONTROLLER_LIST"))).Content["names"].ToObject<IEnumerable<string>>();

    public static async Task<IEnumerable<string>> GetWhiteLists(this OMMSCentralClient client)
        => JsonConvert.DeserializeObject<IEnumerable<string>>((await client.SendAndGetReply<JObject>(new("WHITELIST_LIST"))).Content["whitelists"].ToString());

    public static async Task<IEnumerable<string>> GetWhiteList(this OMMSCentralClient client, string whiteList)
        => JsonConvert.DeserializeObject<IEnumerable<string>>((await client.SendAndGetReply<JObject>(RequestPackage.GetWhiteList(whiteList))).Content["players"].ToString());
}
