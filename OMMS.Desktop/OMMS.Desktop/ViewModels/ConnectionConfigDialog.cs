using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using Newtonsoft.Json;
using OMMS.Core;
using OMMS.Core.Components;
using OMMS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMMS.Desktop.ViewModels;

public partial class ConnectionConfigDialog :ObservableObject
{
    [ObservableProperty]
    private string ipAddress;

    [ObservableProperty]
    private string port;

    [ObservableProperty]
    private string loginCode;

    public Action<string> AddAction { get; set; }

    [RelayCommand]
    public void Cancel(ContentDialog contentDialog) => contentDialog.Hide();

    [RelayCommand]
    public Task Add(ContentDialog contentDialog) => Task.Run(async () =>
    {
        var parameters = new ClientConnectionParameters
        {
            IpAddress = IpAddress,
            Port = int.Parse(Port),
            LoginCode = int.Parse(LoginCode)
        };

        using var omms = new OMMSCentralClient(parameters);

        if (await omms.Connect())
        {
            var systemInfo = await omms.GetSystemInfo();
            await omms.Disconnect();

            App.MainWindow.DispatcherQueue.TryEnqueue(() =>
            {
                AddAction(systemInfo.NetworkInfomation.HostName);
                contentDialog.Hide();
            });
        }
    });
}
