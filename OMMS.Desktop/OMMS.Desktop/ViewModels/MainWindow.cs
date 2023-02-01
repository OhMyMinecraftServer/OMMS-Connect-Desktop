using CommunityToolkit.Mvvm.ComponentModel;
using OMMS.Core;
using OMMS.Core.Components;
using OMMS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMMS.Desktop.ViewModels;

public partial class MainWindow : ObservableObject
{
    public MainWindow() 
    {
        Task.Run(async () =>
        {
            var omms = new OMMSCentralClient(new()
            {
                IpAddress = "127.0.0.1",
                Port = 50000,
                LoginCode = 114514
            });

            await omms.Connect();
            var systemInfo = await omms.GetSystemInfo();
            App.MainWindow.DispatcherQueue.TryEnqueue(() => SystemInfo = systemInfo);
        });
    }

    [ObservableProperty]
    public SystemInfo systemInfo;
}
