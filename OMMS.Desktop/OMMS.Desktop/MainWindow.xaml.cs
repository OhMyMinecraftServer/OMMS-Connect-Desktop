using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using WinUIEx;

namespace OMMS.Desktop;

public sealed partial class MainWindow : WindowEx
{
    public MainWindow()
    {
        this.InitializeComponent();

        AppWindow.Title = "OMMS.Desktop";
        AppWindow.TitleBar.ExtendsContentIntoTitleBar = true;
        AppWindow.TitleBar.ButtonBackgroundColor = Colors.Transparent;
        AppWindow.TitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;

        (MinWidth, MinHeight) = (516, 328);
        //(Width, Height) = (App.Configuration.AppWindowWidth, App.Configuration.AppWindowHeight);
        (Width, Height) = (1050, 550);

        Backdrop = Environment.OSVersion.Version.Build >= 22000
           ? new MicaSystemBackdrop() { Kind = Microsoft.UI.Composition.SystemBackdrops.MicaKind.BaseAlt }
           : new AcrylicSystemBackdrop()
           {
               DarkTintOpacity = 0.75,
               DarkLuminosityOpacity = 0.75,
               DarkTintColor = Colors.Black,
               DarkFallbackColor = Colors.Black
           };
    }

    private void ToggleButton_Click(object sender, RoutedEventArgs e)
    {
        var toggleButton = (ToggleButton)sender;
        var grid = (Grid)toggleButton.Parent;
        var fontIcon = (FontIcon)toggleButton.FindName("FontIcon");

        grid.SetValue(Grid.RowSpanProperty, toggleButton.IsChecked.GetValueOrDefault(false) ? 3 : 1);
        fontIcon.Glyph = toggleButton.IsChecked.GetValueOrDefault(false) ? "\ue70e" : "\ue70d";
    }
}
