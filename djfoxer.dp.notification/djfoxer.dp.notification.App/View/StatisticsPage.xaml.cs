using djfoxer.dp.notification.App.Helpers;
using djfoxer.dp.notification.App.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace djfoxer.dp.notification.App.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StatisticsPage
    {

        public StatisticsViewModel Vm;

        public StatisticsPage()
        {
            this.InitializeComponent();
            Vm = (StatisticsViewModel)DataContext;

            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            ApplicationView.PreferredLaunchViewSize = new Size { Height = 780, Width = 500 };


            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            // Title Bar Content Area
            titleBar.BackgroundColor = Color.FromArgb(0, 91, 150, 56);
            titleBar.ForegroundColor = Colors.White;

            // Title Bar Button Area
            titleBar.ButtonBackgroundColor = Color.FromArgb(0, 91, 150, 56);
            titleBar.ButtonForegroundColor = Colors.White;

            SystemNavigationManager.GetForCurrentView().BackRequested += (s, e) =>
            {
                Vm.GoBackToNotificationsList();
                e.Handled = true;
            };

        }
    }
}
