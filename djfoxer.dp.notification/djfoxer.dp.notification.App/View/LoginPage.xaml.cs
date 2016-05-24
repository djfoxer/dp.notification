using Windows.Foundation;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Navigation;
using Windows.UI;
using djfoxer.dp.notification.App.Helpers;
using Windows.UI.Core;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace djfoxer.dp.notification.App.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage
    {
        public LoginPage()
        {
            this.InitializeComponent();
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
            base.OnNavigatedTo(e);
        }
    }
}
