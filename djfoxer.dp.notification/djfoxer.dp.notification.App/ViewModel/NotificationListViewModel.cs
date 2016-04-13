using djfoxer.dp.notification.App.Model;
using djfoxer.dp.notification.Core;
using djfoxer.dp.notification.Core.Logic;
using djfoxer.dp.notification.Core.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Threading;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.System;
using Windows.System.Threading;
using Windows.UI.Notifications;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace djfoxer.dp.notification.App.ViewModel
{
    public class NotificationListViewModel : ViewModelBase
    {

        #region Service

        private readonly IDataService _dataService;
        private readonly INavigationService _navigationService;
        private readonly IDialogService _dialogService;
        private readonly ToastLogic _toastLogic;


        #endregion

        #region Ctor

        public NotificationListViewModel(
            IDataService dataService,
            INavigationService navigationService,
            IDialogService dialogService,
            ToastLogic toastLogic)
        {
            _dataService = dataService;
            _navigationService = navigationService;
            _dialogService = dialogService;
            _toastLogic = toastLogic;

            ThreadPoolTimer PeriodicTimer = ThreadPoolTimer.CreatePeriodicTimer((source) =>
            {
                LoadNotification();

            }, TimeSpan.FromSeconds(12));

            Application.Current.Resuming += Current_Resuming;
        }

        private void Current_Resuming(object sender, object e)
        {
            LoadNotification();
        }

        #endregion

        #region PROP

        private Notification _SelectedNotification;

        public Notification SelectedNotification
        {
            get
            {
                return _SelectedNotification;
            }
            set
            {
                Set(ref _SelectedNotification, value);
            }
        }


        private ObservableCollection<Notification> _Notifications;

        public ObservableCollection<Notification> Notifications
        {
            get
            {
                return _Notifications;
            }
            set
            {
                Set(ref _Notifications, value);
            }
        }

        #endregion

        #region Methods

        public void LoadNotification()
        {
            Task.Run(async () =>
            {

                var notifications = new ObservableCollection<Notification>(await _dataService.GetNotifications());

                var fresNotifications = _dataService.SaveNotifications(notifications.ToList());

                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    Notifications = notifications;

                    fresNotifications.ToList().ForEach(n => _toastLogic.ShowToast(n));
                 //   _toastLogic.ShowToast(notifications.First());
                });



            });

        }
        #endregion

        #region Commands

        private RelayCommand _openLink;

        public RelayCommand OpenLink
        {
            get
            {
                return _openLink ?? (_openLink = new RelayCommand(async () =>
              {
                  var notification = SelectedNotification;
                  if (notification != null && !string.IsNullOrEmpty(notification.TargetUrl))
                  {
                      await Launcher.LaunchUriAsync(new Uri(notification.TargetUrl));
                      _dataService.SetNotificationAsOld(notification.Id);
                  }
              }));

            }
        }

        private RelayCommand _Logout;

        public RelayCommand Logout
        {
            get
            {
                return _Logout ?? (_Logout = new RelayCommand(() =>
               {
                   _dialogService.ShowMessage("Czy chcesz się wylogować?", "Wylogowanie", "Tak", "Nie", (confirm) =>
                   {
                       if (confirm)
                       {
                           _dataService.Logout();
                           _navigationService.NavigateTo(ViewModelLocator.LoginPageKey);
                       }
                   });


               }));

            }
        }

        private RelayCommand _NotificationDetails;

        public RelayCommand NotificationDetails
        {
            get
            {
                return _NotificationDetails ?? (_NotificationDetails = new RelayCommand(() =>
                {
                    var notification = SelectedNotification;
                    if (notification != null)
                    {
                        var menu = new PopupMenu();
                        if (!string.IsNullOrEmpty(notification.TargetUrl))
                        {
                            menu.Commands.Add(new UICommand("Otwórz link", async (command) =>
                            {
                                await Launcher.LaunchUriAsync(new Uri(notification.TargetUrl));
                            }));
                        }
                        if (notification.Status == Core.Enum.NotificationStatus.New)
                        {
                            menu.Commands.Add(new UICommand("Ustaw jako przeczytany", (command) =>
                            {
                                notification.Status = Core.Enum.NotificationStatus.Old;
                                Notifications = _Notifications;
                            }));
                        }

                        if (notification.Status == Core.Enum.NotificationStatus.Old)
                        {
                            menu.Commands.Add(new UICommand("Usuń", (command) =>
                            {
                                Notifications = new ObservableCollection<Notification>(_Notifications.Where(x => x.Id != notification.Id).ToList());
                            }));
                        }

                        //  var chosenCommand = await menu.ShowForSelectionAsync(GetElementRect((FrameworkElement)sender));
                    }


                }));

            }
        }

        public async void OpenDetailsMenu(object sender)
        {
            var notification = SelectedNotification;
            if (notification != null)
            {
                var menu = new PopupMenu();
                if (!string.IsNullOrEmpty(notification.TargetUrl))
                {
                    menu.Commands.Add(new UICommand("Otwórz link", async (command) =>
                    {
                        await Launcher.LaunchUriAsync(new Uri(notification.TargetUrl));
                    }));
                }
                if (notification.Status == Core.Enum.NotificationStatus.New)
                {
                    menu.Commands.Add(new UICommand("Ustaw jako przeczytany", (command) =>
                    {
                        notification.Status = Core.Enum.NotificationStatus.Old;
                        Notifications = _Notifications;
                    }));
                }

                if (notification.Status == Core.Enum.NotificationStatus.Old)
                {
                    menu.Commands.Add(new UICommand("Usuń", (command) =>
                    {
                        Notifications = new ObservableCollection<Notification>(_Notifications.Where(x => x.Id != notification.Id).ToList());
                    }));
                }

                var chosenCommand = await menu.ShowForSelectionAsync(GetElementRect((FrameworkElement)sender));
            }
        }

        public Rect GetElementRect(FrameworkElement element)
        {
            GeneralTransform buttonTransform = element.TransformToVisual(null);
            Point point = buttonTransform.TransformPoint(new Point());
            return new Rect(point, new Size(element.ActualWidth, element.ActualHeight));
        }

        #endregion
    }
}
