using djfoxer.dp.notification.App.Model;
using djfoxer.dp.notification.App.View;
using djfoxer.dp.notification.Core.Logic;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace djfoxer.dp.notification.App.ViewModel
{
    public class ViewModelLocator
    {
        public const string NotificationListPageKey = "NotificationListPageKey";
        public const string LoginPageKey = "LoginPageKey";

        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            var nav = new NavigationService();
            nav.Configure(NotificationListPageKey, typeof(NotificationListPage));
            nav.Configure(LoginPageKey, typeof(LoginPage));

            SimpleIoc.Default.Register<INavigationService>(() => nav);

            SimpleIoc.Default.Register<IDialogService, DialogService>();
            SimpleIoc.Default.Register<IStorageService, StorageService>();
            SimpleIoc.Default.Register<ToastLogic>();

            if (ViewModelBase.IsInDesignModeStatic)
            {
                SimpleIoc.Default.Register<IDataService, Design.DesignDataService>();
            }
            else
            {
                SimpleIoc.Default.Register<IDataService, DataService>();
            }

            SimpleIoc.Default.Register<LoginViewModel>();
            SimpleIoc.Default.Register<NotificationListViewModel>();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]

        public LoginViewModel Login => ServiceLocator.Current.GetInstance<LoginViewModel>();
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public NotificationListViewModel NotyficationList => ServiceLocator.Current.GetInstance<NotificationListViewModel>();
    }
}
