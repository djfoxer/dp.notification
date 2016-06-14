using djfoxer.dp.notification.App.Helpers;
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
    public sealed partial class NotificationListPage : BasePage
    {

        public NotificationListViewModel Vm;

        public NotificationListPage()
        {
            this.InitializeComponent();
            HideBackButton();
            Vm = (NotificationListViewModel)DataContext;

            SystemNavigationManager.GetForCurrentView().BackRequested += (s, e) =>
            {
                e.Handled = false;
            };
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Vm.LoadNotification();
            base.OnNavigatedTo(e);
            this.Frame.BackStack.Clear();

        }

        private void Grid_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            Vm.OpenDetailsMenu(e.OriginalSource);
        }
    }
}
