using djfoxer.dp.notification.App.Model;
using djfoxer.dp.notification.Core.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Xaml;

namespace djfoxer.dp.notification.App.ViewModel
{
    public class StatisticsViewModel : ViewModelBase
    {

        #region Methods

        public void GoBackToNotificationsList()
        {
            _navigationService.NavigateTo(ViewModelLocator.NotificationListPageKey);
        }

        private RelayCommand _StartAnalysis;

        public RelayCommand StartAnalysis
        {
            get
            {
                return _StartAnalysis ?? (_StartAnalysis = new RelayCommand(async () =>
                {
                    if (BlogStatistics != null)
                        BlogStatistics = BlogStatistics.Clear();
                    ShowStat = Visibility.Collapsed;
                    IsAnalysing = true;
                    var newData = await _dataService.GetFullBlogStatistics();
                    if (SelectedOrder != null)
                        newData.Sort(SelectedOrder.Id);
                    BlogStatistics = newData;
                    IsAnalysing = false;
                    ShowStat = Visibility.Visible;

                }));

            }
        }

        private RelayCommand _OpenLink;

        public RelayCommand OpenLink
        {
            get
            {
                return _OpenLink ?? (_OpenLink = new RelayCommand(async () =>
                {
                    if (SelectedPost != null && !string.IsNullOrWhiteSpace(SelectedPost.Id))
                    {
                        await Launcher.LaunchUriAsync(new Uri("http://dp.do/" + SelectedPost.Id));
                    }

                }));

            }
        }


        private RelayCommand<object> _ChangeSortOrder;

        public RelayCommand<object> ChangeSortOrder
        {
            get
            {
                return _ChangeSortOrder ?? (_ChangeSortOrder = new RelayCommand<object>((object o) =>
                 {
                     if (SelectedOrder != null)
                     {
                         var sorted  = BlogStatistics.Sort(SelectedOrder.Id);
                         BlogStatistics = null;
                         BlogStatistics = sorted;

                     }

                 }));

            }
        }


        #endregion

        #region Prop

        private List<SelectedOrder> _OrderList;

        public List<SelectedOrder> OrderList
        {
            get
            {
                return _OrderList;
            }
            set
            {
                Set(ref _OrderList, value);
            }
        }

        private SelectedOrder _SelectedOrder;

        public SelectedOrder SelectedOrder
        {
            get
            {
                return _SelectedOrder;
            }
            set
            {
                Set(ref _SelectedOrder, value);
            }
        }

        private BlogStatistic _BlogStatistics;

        public BlogStatistic BlogStatistics
        {
            get
            {
                return _BlogStatistics;
            }
            set
            {
                Set(ref _BlogStatistics, value);
            }
        }

        private Post _SelectedPost;

        public Post SelectedPost
        {
            get
            {
                return _SelectedPost;
            }
            set
            {
                Set(ref _SelectedPost, value);
            }
        }

        private bool _IsAnalysing;

        public bool IsAnalysing
        {
            get
            {
                return _IsAnalysing;
            }
            set
            {
                Set(ref _IsAnalysing, value);
            }
        }

        private Visibility _ShowStat;

        public Visibility ShowStat
        {
            get
            {
                return _ShowStat;
            }
            set
            {
                Set(ref _ShowStat, value);
            }
        }

        #endregion

        #region Service

        private readonly IDataService _dataService;
        private readonly INavigationService _navigationService;
        private readonly IDialogService _dialogService;

        #endregion

        #region Ctor

        public StatisticsViewModel(
            IDataService dataService,
            INavigationService navigationService,
            IDialogService dialogService)
        {
            _dataService = dataService;
            _navigationService = navigationService;
            _dialogService = dialogService;

            ShowStat = Visibility.Collapsed;

            OrderList = new List<SelectedOrder>() {
                new SelectedOrder(0, "wyświetlenia"),
                new SelectedOrder(1,"komentarze"),
                new SelectedOrder(2,"data publikacji") };
            SelectedOrder = OrderList.LastOrDefault();
        }



        #endregion
    }

}
