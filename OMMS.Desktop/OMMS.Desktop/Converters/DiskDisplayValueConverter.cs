using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.UI.Xaml.Data;
using System.Threading.Tasks;
using OMMS.Core.Models;
using Natsurainko.Toolkits.Values;

namespace OMMS.Desktop.Converters;

public class DiskDisplayValueConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is SystemInfo.FileSystemInfo.DiskInfo diskInfo)
            return new
            {
                Free = diskInfo.Free.FormatSize(),
                Used = (diskInfo.Total - diskInfo.Free).FormatSize(),
                Total = diskInfo.Total.FormatSize()
            };

        return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
