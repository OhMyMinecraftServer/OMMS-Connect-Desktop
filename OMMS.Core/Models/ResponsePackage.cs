using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMMS.Core.Models;

public class ResponsePackage
{
    [JsonProperty("responseCode")]
    public string ResponseCode { get; set; }
}

public class ResponsePackage<TClass> : ResponsePackage
{
    [JsonProperty("content")]
    public TClass Content { get; set; }
}