using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMMS.Core.Models;

public class RequestPackage
{
    public RequestPackage() { }

    public RequestPackage(string request) { Request = request; }

    [JsonProperty("version")]
    public uint ProtocolVersion { get; set; } = 0xc0000002;

    [JsonProperty("request")]
    public string Request { get; set; }

    public static RequestPackage<object> Ping(string connectionKey)
        => new()
        {
            Request = "PING",
            Content = new
            {
                token = connectionKey
            }
        };

    public static RequestPackage<object> GetWhiteList(string whiteList)
        => new()
        {
            Request = "WHITELIST_GET",
            Content = new
            {
                whitelist = whiteList
            }
        };
}

public class RequestPackage<TClass> : RequestPackage
{
    public RequestPackage() { }

    public RequestPackage(string request) { Request = request; }

    [JsonProperty("content")]
    public TClass Content { get; set; }
}
