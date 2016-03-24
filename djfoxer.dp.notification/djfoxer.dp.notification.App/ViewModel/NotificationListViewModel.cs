using djfoxer.dp.notification.App.Model;
using djfoxer.dp.notification.Core;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Threading;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace djfoxer.dp.notification.App.ViewModel
{
    public class NotificationListViewModel : ViewModelBase
    {

        #region Service

        private readonly IDataService _dataService;
        private readonly INavigationService _navigationService;

        #endregion

        #region Ctor

        public NotificationListViewModel(
            IDataService dataService,
            INavigationService navigationService)
        {
            _dataService = dataService;
            _navigationService = navigationService;
        }

        #endregion

        #region PROP
        private ObservableCollection<Notification> _notyfications;

        public ObservableCollection<Notification> Notyfications
        {
            get
            {
                return _notyfications;
            }
            set
            {
                Set(ref _notyfications, value);
            }
        }

        #endregion

        #region Methods

        public void LoadNotyfication()
        {
            Task.Run(async () =>
            {

                var notifications = new ObservableCollection<Notification>(await _dataService.GetNotifications());

                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    Notyfications = notifications;
                });


            });

            #endregion
        }
    }
}
