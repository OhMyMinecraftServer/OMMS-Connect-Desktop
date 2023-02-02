using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using OMMS.Desktop.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace OMMS.Desktop.Components;

public partial class Configuration : ObservableObject
{
    public static Configuration Load()
    => Container.Values.Any()
    ? Create()
    : Default();

    public static Configuration Create()
    {
        var configuration = new Configuration();

        foreach (var key in Container.Values.Keys)
        {
            var property = configuration.GetType().GetProperty(key);
            property.SetValue(configuration, JsonConvert.DeserializeObject((string)Container.Values[key], property.PropertyType));
        }

        return configuration;
    }

    public static Configuration Default()
        => new() { ConnectionConfigs = new() };

    public static ApplicationDataContainer Container => ApplicationData.Current.LocalSettings;

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);

        Container.Values[e.PropertyName] = JsonConvert.SerializeObject(GetType().GetProperty(e.PropertyName).GetValue(this));
    }

    public void ReportPropertyChanged(PropertyChangedEventArgs e)
        => OnPropertyChanged(e);
}

public partial class Configuration
{
    [ObservableProperty]
    [JsonIgnore]
    private ConnectionConfig currentConnectionConfig;

    [ObservableProperty]
    [JsonIgnore]
    private List<ConnectionConfig> connectionConfigs;
}