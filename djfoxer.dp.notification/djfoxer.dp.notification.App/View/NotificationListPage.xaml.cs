﻿using djfoxer.dp.notification.App.Helpers;
using djfoxer.dp.notification.App.ViewModel;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace djfoxer.dp.notification.App.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NotificationListPage
    {

        public NotificationListViewModel Vm;

        public NotificationListPage()
        {
            this.InitializeComponent();
            Vm = (NotificationListViewModel)DataContext;
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Vm.LoadNotification();
            base.OnNavigatedTo(e);


        }

        private void Grid_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            Vm.OpenDetailsMenu(e.OriginalSource);
        }
    }
}
