using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using OMMS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMMS.Desktop.Models;

public partial class ConnectionConfig : ObservableObject
{
    [ObservableProperty]
    [JsonIgnore]
    private string displayName;

    public ClientConnectionParameters ConnectionParameters { get; set; }

    public override bool Equals(object obj)
    {
        if (obj is ConnectionConfig config)
            return ConnectionParameters.Equals(config.ConnectionParameters);
        
        return false;
    }

    public override int GetHashCode()
    {
        return ConnectionParameters.GetHashCode();
    }
}
