using Natsurainko.Toolkits.Text;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using OMMS.Core;
using Newtonsoft.Json.Linq;
using OMMS.Core.Models;
using OMMS.Core.Components;

using var omms = new OMMSCentralClient(new()
{
    IpAddress = "127.0.0.1",
    Port = 50000,
    LoginCode = 114514
});

await omms.Connect();

var whiteLists = await omms.GetWhiteLists();
var whiteList = await omms.GetWhiteList(whiteLists.First());

await omms.Disconnect();

Console.ReadKey();