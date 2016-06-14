using System;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;

namespace djfoxer.dp.notification.App.Helpers
{
    public class BasePage : Page
    {
        public BasePage()
        {
            HideStatusBar();

            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            ApplicationView.PreferredLaunchViewSize = new Size { Height = 780, Width = 500 };



            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            // Title Bar Content Area
            titleBar.BackgroundColor = Color.FromArgb(0, 91, 150, 56);
            titleBar.ForegroundColor = Colors.White;

            // Title Bar Button Area
            titleBar.ButtonBackgroundColor = Color.FromArgb(0, 91, 150, 56);
            titleBar.ButtonForegroundColor = Colors.White;

        }

        protected void HideBackButton()
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
        }

        protected void ShowBackButton()
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
        }

        private async void HideStatusBar()
        {
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
            {
                var statusBar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
                await statusBar.HideAsync();
            }
        }
    }
}
