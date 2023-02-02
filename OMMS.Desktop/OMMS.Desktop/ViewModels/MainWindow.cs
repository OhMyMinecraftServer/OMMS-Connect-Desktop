using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using OMMS.Core;
using OMMS.Core.Components;
using OMMS.Core.Models;
using OMMS.Desktop.Models;
using OMMS.Desktop.Views.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMMS.Desktop.ViewModels;

public partial class MainWindow : ObservableObject
{
    public MainWindow() 
    {
        ConnectionConfigs = new (App.Configuration.ConnectionConfigs);
        CurrentConnectionConfig = App.Configuration.CurrentConnectionConfig;

        ConnectionConfigs.CollectionChanged += ConnectionConfigs_CollectionChanged;
    }

    public OMMSCentralClient OMMSCentralClient { get; private set; }

    [ObservableProperty]
    private bool connected = false;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ConnectButtonCommand))]
    private ConnectionConfig currentConnectionConfig;

    [ObservableProperty]
    private string currentConnectionName = "No Server";

    [ObservableProperty]
    private string connectState = "Disconnected";

    [ObservableProperty]
    private ObservableCollection<ConnectionConfig> connectionConfigs;

    [ObservableProperty]
    private SystemInfo systemInfo;

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);

        if (e.PropertyName == nameof(CurrentConnectionConfig))
        {
            CurrentConnectionName = CurrentConnectionConfig != null
                ? CurrentConnectionConfig.DisplayName
                : "No Server";

            App.Configuration.CurrentConnectionConfig = CurrentConnectionConfig;
        }
    }

    private void ConnectionConfigs_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        App.Configuration.ConnectionConfigs = ConnectionConfigs.ToList();
    }

    private bool EnableConnect() => CurrentConnectionConfig != null;

    [RelayCommand(CanExecute = nameof(EnableConnect))]
    public Task ConnectButton() => Task.Run(async () =>
    {
        if (Connected)
        {
            App.MainWindow.DispatcherQueue.TryEnqueue(() => ConnectState = "Diconnecting");

            await OMMSCentralClient.Disconnect();
            OMMSCentralClient.Dispose();

            App.MainWindow.DispatcherQueue.TryEnqueue(() =>
            {
                ConnectState = "Disconnected";
                SystemInfo = null;
                Connected = false;
            });

            return;
        }

        App.MainWindow.DispatcherQueue.TryEnqueue(() => ConnectState = "Connecting");

        OMMSCentralClient = new OMMSCentralClient(CurrentConnectionConfig.ConnectionParameters);

        if (await OMMSCentralClient.Connect())
        {
            var systemInfo = await OMMSCentralClient.GetSystemInfo();

            App.MainWindow.DispatcherQueue.TryEnqueue(() =>
            {
                SystemInfo = systemInfo;
                ConnectState = "Connected";
                Connected = true;
            });
        }
    });

    [RelayCommand]
    public async void AddConnectionConfig()
    {
        var connectionConfigDialog = new Views.Dialogs.ConnectionConfigDialog();
        connectionConfigDialog.XamlRoot = Desktop.MainWindow.XamlRoot;
        
        var viewModel= new ConnectionConfigDialog();
        viewModel.AddAction = hostName => ConnectionConfigs.Add(new ConnectionConfig
        {
            ConnectionParameters = new ClientConnectionParameters
            {
                IpAddress = viewModel.IpAddress,
                LoginCode = int.Parse(viewModel.LoginCode),
                Port = int.Parse(viewModel.Port)
            },
            DisplayName = hostName
        });

        connectionConfigDialog.DataContext = viewModel;

        await connectionConfigDialog.ShowAsync();
    }
}
