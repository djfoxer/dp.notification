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
using static djfoxer.dp.notification.Core.Enum;

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

            LoadingScreen = Visibility.Visible;
            NoNotifications = Visibility.Collapsed;
            ShowNotifications = Visibility.Collapsed;

            ThreadPoolTimer PeriodicTimer = ThreadPoolTimer.CreatePeriodicTimer((source) =>
            {
                LoadNotification();

            }, TimeSpan.FromSeconds(30));

            Application.Current.Resuming += Current_Resuming;
        }

        private void Current_Resuming(object sender, object e)
        {
            LoadNotification();
        }

        #endregion

        #region PROP

        private Visibility _LoadingScreen;

        public Visibility LoadingScreen
        {
            get
            {
                return _LoadingScreen;
            }
            set
            {
                Set(ref _LoadingScreen, value);
            }
        }

        private Visibility _NoNotifications;

        public Visibility NoNotifications
        {
            get
            {
                return _NoNotifications;
            }
            set
            {
                Set(ref _NoNotifications, value);
            }
        }

        private Visibility _ShowNotifications;

        public Visibility ShowNotifications
        {
            get
            {
                return _ShowNotifications;
            }
            set
            {
                Set(ref _ShowNotifications, value);
            }
        }

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

        //to prevent list skipping and use animation to modify list
        private void RefreshNotificationsList(List<Notification> freshList)
        {
            if (Notifications == null || Notifications.Count == 0)
            {
                Notifications = new ObservableCollection<Notification>(freshList);
            }
            else
            {
                if (freshList == null || freshList.Count == 0)
                {
                    Notifications = new ObservableCollection<Notification>();
                }
                else
                {
                    List<string> existId = Notifications.Select(x => x.Id).ToList();

                    //remove
                    var toRemove = existId.Where(x => !freshList.Exists(n => n.Id == x));

                    foreach (var rem in toRemove)
                    {
                        Notifications.Remove(Notifications.Where(x => x.Id == rem).FirstOrDefault());
                    }

                    //add
                    var toAdd = freshList.Where(x => !existId.Exists(n => n == x.Id)).ToList();

                    var toAddNew = toAdd.Where(x => x.Status == NotificationStatus.New).ToList().OrderBy(x => x.AddedDate);
                    foreach (var newItem in toAddNew)
                    {
                        Notifications.Insert(0, newItem);
                    }

                    var toAddOld = toAdd.Where(x => x.Status == NotificationStatus.Old).ToList().OrderBy(x => x.AddedDate);
                    var lastOld = Notifications.LastOrDefault(x => x.Status == NotificationStatus.New);
                    int firstOldIndex = 0;
                    if (lastOld != null)
                    {
                        firstOldIndex = Notifications.IndexOf(lastOld);
                    }

                    foreach (var newItem in toAddNew)
                    {
                        Notifications.Insert(firstOldIndex, newItem);
                    }

                    //update rest
                    var toUdpate = freshList.Where(x => !toAdd.Exists(n => n.Id == x.Id)).ToList();

                    foreach (var up in toUdpate)
                    {
                        var toUpOne = Notifications.Where(x => x.Id == up.Id && x.Status != up.Status).FirstOrDefault();
                        if (toUpOne != null)
                        {
                            int index = Notifications.IndexOf(toUpOne);
                            Notifications.RemoveAt(index);
                            Notifications.Insert(firstOldIndex, up);
                        }
                    }
                }
            }
        }

        private void RefreshView()
        {
            if (LoadingScreen == Visibility.Visible)
            {
                LoadingScreen = Visibility.Collapsed;
            }

            if (Notifications != null && Notifications.Count > 0)
            {
                NoNotifications = Visibility.Collapsed;
                ShowNotifications = Visibility.Visible;
            }
            else
            {
                NoNotifications = Visibility.Visible;
                ShowNotifications = Visibility.Collapsed;
            }
        }

        public void LoadNotification()
        {
            Task.Run(async () =>
            {
                _dataService.RefreshData();
                var notifications = await _dataService.GetNotifications();

                var fresNotifications = _dataService.SaveNotifications(notifications);

                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    RefreshNotificationsList(notifications);


                    fresNotifications.ToList().ForEach(n => _toastLogic.ShowToast(n, true));
                    //_toastLogic.ShowToast(notifications.First(), true);

                    RefreshView();
                });



            });

        }
        #endregion



        #region Commands



        private RelayCommand _OpenDPLink;

        public RelayCommand OpenDPLink
        {
            get
            {
                return _OpenDPLink ?? (_OpenDPLink = new RelayCommand(async () =>
                {
                    await Launcher.LaunchUriAsync(new Uri(Const.UrlFullAddress));
                }));

            }
        }

        private RelayCommand _DePeszaInfo;

        public RelayCommand DePeszaInfo
        {
            get
            {
                return _DePeszaInfo ?? (_DePeszaInfo = new RelayCommand(async () =>
                {
                    await _dialogService.ShowMessage("DePesza\n\nAutor aplikacji: Grzegorz \"djfoxer\" Jamiołkowski\nLogo: Jarek Uliczka ", "Informacje");
                }));

            }
        }

        private RelayCommand _OpenStatistics;

        public RelayCommand OpenStatistics
        {
            get
            {
                return _OpenStatistics ?? (_OpenStatistics = new RelayCommand(() =>
               {
                   _navigationService.NavigateTo(ViewModelLocator.StatisticsPage);
               }));

            }
        }

        private RelayCommand _RemoveSelected;

        public RelayCommand RemoveSelected
        {
            get
            {
                return _RemoveSelected ?? (_RemoveSelected = new RelayCommand(async () =>
                {
                    var notification = SelectedNotification;
                    if (notification != null)
                    {

                        await _dialogService.ShowMessage("Czy chcesz się usunąc powidomienie?", "Usunięcie", "Tak", "Nie", (confirm) =>
                         {
                             if (confirm)
                             {
                                 Notifications.Remove(notification);
                                 _dataService.RemoveNotyfication(notification.Id);
                                 RefreshView();
                             }
                         });



                    }
                }));

            }
        }

        private RelayCommand _openLink;

        public RelayCommand OpenLink
        {
            get
            {
                return _openLink ?? (_openLink = new RelayCommand(async () =>
              {
                  var notification = SelectedNotification;
                  if (notification != null)
                  {
                      if (!string.IsNullOrEmpty(notification.TargetUrl))
                      {
                          await Launcher.LaunchUriAsync(new Uri(notification.TargetUrl));
                      }
                      //if (notification.Status == Core.Enum.NotificationStatus.New)
                      //{
                      //    if (!string.IsNullOrEmpty(notification.TargetUrl))
                      //    {
                      //        await Launcher.LaunchUriAsync(new Uri(notification.TargetUrl));
                      //    }
                      //    Notifications = new ObservableCollection<Notification>(await _dataService.SetNotificationAsOld(notification.Id));
                      //}
                      //else
                      //{
                      //    await _dialogService.ShowMessage("Czy chcesz się usunąc powidomienie?", "Usunięcie", "Tak", "Nie", (confirm) =>
                      //     {
                      //         if (confirm)
                      //         {
                      //             Notifications.Remove(notification);
                      //             _dataService.RemoveNotyfication(notification.Id);
                      //         }
                      //     });

                      //}


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

        private RelayCommand _RemoveOld;

        public RelayCommand RemoveOld
        {
            get
            {
                return _RemoveOld ?? (_RemoveOld = new RelayCommand(() =>
                {
                    _dialogService.ShowMessage("Czy chcesz usunąć wszystkie przeczytane powiadomienia?", "Usunięcie", "Tak", "Nie", (confirm) =>
                    {
                        if (confirm)
                        {
                            var oldNotifications = Notifications.Where(x => x.Status == Core.Enum.NotificationStatus.Old).ToList();
                            foreach (var oldItem in oldNotifications)
                            {
                                _dataService.RemoveNotyfication(oldItem.Id);
                                Notifications.Remove(oldItem);
                            }

                            RefreshView();
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
                                if (!string.IsNullOrEmpty(notification.TargetUrl))
                                {
                                    await Launcher.LaunchUriAsync(new Uri(notification.TargetUrl));
                                }
                            }));
                        }
                        if (notification.Status == Core.Enum.NotificationStatus.New)
                        {
                            menu.Commands.Add(new UICommand("Ustaw jako przeczytany", async (command) =>
                            {
                                Notifications = new ObservableCollection<Notification>(await _dataService.SetNotificationAsOld(notification.Id));

                            }));
                        }


                        menu.Commands.Add(new UICommand("Usuń", async (command) =>
                        {
                            await _dialogService.ShowMessage("Czy chcesz się usunąc powidomienie?", "Usunięcie", "Tak", "Nie", (confirm) =>
                            {
                                if (confirm)
                                {
                                    Notifications.Remove(notification);
                                    _dataService.RemoveNotyfication(notification.Id);
                                }
                            });
                        }));


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
                        if (!string.IsNullOrEmpty(notification.TargetUrl))
                        {
                            await Launcher.LaunchUriAsync(new Uri(notification.TargetUrl));
                        }
                        Notifications = new ObservableCollection<Notification>(await _dataService.SetNotificationAsOld(notification.Id));

                    }));
                }
                if (notification.Status == Core.Enum.NotificationStatus.New)
                {
                    menu.Commands.Add(new UICommand("Ustaw jako przeczytany", async (command) =>
                    {

                        Notifications = new ObservableCollection<Notification>(await _dataService.SetNotificationAsOld(notification.Id));
                    }));
                }

                menu.Commands.Add(new UICommand("Usuń", async (command) =>
                {
                    await _dialogService.ShowMessage("Czy chcesz się usunąc powidomienie?", "Usunięcie", "Tak", "Nie", (confirm) =>
                    {
                        if (confirm)
                        {
                            Notifications.Remove(notification);
                            _dataService.RemoveNotyfication(notification.Id);
                            RefreshView();
                        }
                    });
                }));


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
