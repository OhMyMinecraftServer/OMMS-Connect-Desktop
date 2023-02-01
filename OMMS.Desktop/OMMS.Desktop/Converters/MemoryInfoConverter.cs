using Microsoft.UI.Xaml.Data;
using Natsurainko.Toolkits.Values;
using OMMS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMMS.Desktop.Converters;

public class MemoryInfoConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is SystemInfo.MemoryInfo info)
        {
            double mp = (double)info.MemoryUsed / info.MemoryTotal;
            double sp = (double)info.SwapUsed / info.SwapTotal;

            return new
            {
                MemoryPercentage = mp,
                SwapPercentage = sp,
                Memory = $"{mp * 100:0.0}%",
                Swap = $"{sp * 100:0.0}%",
                MemoryUsed = info.MemoryUsed.FormatSize(),
                MemoryTotal = info.MemoryTotal.FormatSize(),
                SwapUsed = info.SwapUsed.FormatSize(),
                SwapTotal = info.SwapTotal.FormatSize()
            };
        }

        return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
