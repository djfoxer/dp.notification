using Windows.Foundation;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Navigation;
using Windows.UI;
using Windows.UI.Core;
using djfoxer.dp.notification.App.Helpers;
using System.Threading.Tasks;
using System;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace djfoxer.dp.notification.App.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : BasePage
    {
        public LoginPage()
        {
            this.InitializeComponent();
            HideBackButton();

            SystemNavigationManager.GetForCurrentView().BackRequested += (s, e) =>
            {
                e.Handled = false;
            };
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            this.Frame.BackStack.Clear();

        }


    }
}
