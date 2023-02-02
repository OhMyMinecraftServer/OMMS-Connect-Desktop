using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMMS.Core.Models;

public struct ClientConnectionParameters
{
    public int LoginCode { get; set; }

    public string IpAddress { get; set; }

    public int Port { get; set; }
}