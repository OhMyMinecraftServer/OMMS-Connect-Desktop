using System;
using System.Runtime.InteropServices;
using System.Windows;
using Windows.System;
using Windows.UI;
using Windows.UI.Popups;
using Microsoft.UI.Composition;
using Microsoft.UI.Composition.SystemBackdrops;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using OMMS_Connect_Desktop;
using WinRT;
using Microsoft.UI.Xaml.Media;
using OMMSClientCoreCSharp;
using OMMSClientCoreCSharp.Session;

namespace OMMS_Connect_Desktop;

internal class WindowsSystemDispatcherQueueHelper
{
    private object _mDispatcherQueueController;
    [DllImport("CoreMessaging.dll")]
    private static extern int CreateDispatcherQueueController([In] DispatcherQueueOptions options,
        [In] [Out] [MarshalAs(UnmanagedType.IUnknown)] ref object dispatcherQueueController);
    public void EnsureWindowsSystemDispatcherQueueController()
    {
        if (DispatcherQueue.GetForCurrentThread() != null)
            return;
        if (_mDispatcherQueueController == null)
        {
            DispatcherQueueOptions options;
            options.dwSize = Marshal.SizeOf(typeof(DispatcherQueueOptions));
            options.threadType = 2;
            options.apartmentType = 2;
            CreateDispatcherQueueController(options, ref _mDispatcherQueueController);
        }
    }
    [StructLayout(LayoutKind.Sequential)]
    private struct DispatcherQueueOptions
    {
        internal int dwSize;
        internal int threadType;
        internal int apartmentType;
    }
}

public sealed partial class MainWindow : Window
{
    private MicaController _desktopAcrylicController;
    private SystemBackdropConfiguration _systemBackdropConfiguration;

    private WindowsSystemDispatcherQueueHelper _windowsSystemDispatcherQueueHelper;

    public MainWindow()
    {
        InitializeComponent();
        TrySetSystemBackdrop();
        ExtendsContentIntoTitleBar = true;
        SetTitleBar(AppTitleBar);
        Activated += Window_Activated;
    }

    #region UI

    private bool TrySetSystemBackdrop()
    {
        if (MicaController.IsSupported())
        {
            _windowsSystemDispatcherQueueHelper = new WindowsSystemDispatcherQueueHelper();
            _windowsSystemDispatcherQueueHelper.EnsureWindowsSystemDispatcherQueueController();
            _systemBackdropConfiguration = new SystemBackdropConfiguration();
            Activated += Window_Activated;
            Closed += Window_Closed;
            ((FrameworkElement)Content).ActualThemeChanged += Window_ThemeChanged;
            _systemBackdropConfiguration.IsInputActive = true;
            SetConfigurationSourceTheme();
            _desktopAcrylicController = new MicaController()
            {
                Kind = MicaKind.BaseAlt
            };
            _desktopAcrylicController.AddSystemBackdropTarget(this.As<ICompositionSupportsSystemBackdrop>());
            _desktopAcrylicController.SetSystemBackdropConfiguration(_systemBackdropConfiguration);
            return true;
        }

        return false;
    }

    private void Window_Activated(object sender, WindowActivatedEventArgs args)
    {
        _systemBackdropConfiguration.IsInputActive = args.WindowActivationState != WindowActivationState.Deactivated;
        if (args.WindowActivationState == WindowActivationState.Deactivated)
        {
            AppTitleTextBlock.Foreground =
                (SolidColorBrush)App.Current.Resources["WindowCaptionForegroundDisabled"];
        }
        else
        {
            AppTitleTextBlock.Foreground =
                (SolidColorBrush)App.Current.Resources["WindowCaptionForeground"];
        }
    }

    private void Window_Closed(object sender, WindowEventArgs args)
    {
        if (_desktopAcrylicController != null)
        {
            _desktopAcrylicController.Dispose();
            _desktopAcrylicController = null;
        }

        Activated -= Window_Activated;
        _systemBackdropConfiguration = null;
    }

    private void Window_ThemeChanged(FrameworkElement sender, object args)
    {
        if (_systemBackdropConfiguration != null) SetConfigurationSourceTheme();
    }

    private void SetConfigurationSourceTheme()
    {
        switch (((FrameworkElement)Content).ActualTheme)
        {
            case ElementTheme.Dark:
                _systemBackdropConfiguration.Theme = SystemBackdropTheme.Dark;
                break;
            case ElementTheme.Light:
                _systemBackdropConfiguration.Theme = SystemBackdropTheme.Light;
                break;
            case ElementTheme.Default:
                _systemBackdropConfiguration.Theme = SystemBackdropTheme.Default;
                break;
        }
    }


    #endregion

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e){
        try
        {
            SessionClient session = InitialSessionClient.Connect(textIP.Text, Int32.Parse(textPort.Text),
                Int32.Parse(textCode.Password));
            session.Close();
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            new MessageBox().setContent("YOU SUCK", exception.Message + "\n" + exception.StackTrace).Activate();
            //await new MessageDialog(exception.Message + "\n" + exception.StackTrace, "YOU SUCKS").ShowAsync();
        }
    }
}