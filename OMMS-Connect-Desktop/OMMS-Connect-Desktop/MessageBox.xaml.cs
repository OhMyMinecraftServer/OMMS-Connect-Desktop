using Microsoft.UI.Composition.SystemBackdrops;
using Microsoft.UI.Composition;
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
using WinRT;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using System.Runtime.InteropServices;
using Windows.System;

namespace OMMS_Connect_Desktop
{

    public sealed partial class MessageBox : Window
    {

        private MicaController _desktopAcrylicController;
        private SystemBackdropConfiguration _systemBackdropConfiguration;

        private WindowsSystemDispatcherQueueHelper _windowsSystemDispatcherQueueHelper;


        public MessageBox()
        {
            this.InitializeComponent();
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
        public MessageBox setContent(String title, String message)
        {
            this.AppTitleTextBlock.Text = title;
            TextBlock.Text = message;
            return this;
        }


    }

    

}
