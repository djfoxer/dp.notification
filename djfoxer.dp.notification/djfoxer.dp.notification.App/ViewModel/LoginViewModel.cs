using djfoxer.dp.notification.App.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace djfoxer.dp.notification.App.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {

        #region Commands

        private async Task<bool> Signin()
        {
            IsActive = false;
            var loginResult = await _dataService.Login(TxtLogin, TxtPassword);

            if (loginResult)
            {
                TxtPassword = string.Empty;
                _navigationService.NavigateTo(ViewModelLocator.NotificationListPageKey);
            }
            else
            {
                var dialog = ServiceLocator.Current.GetInstance<IDialogService>();
                await dialog.ShowMessage("Błędny login/hasło", "Logowanie");

            }
            IsActive = true;

            return true;
        }

        private RelayCommand _login;

        public RelayCommand Login
        {
            get
            {
                return _login ?? (_login = new RelayCommand(async () =>
                {
                    await Signin();
                }));

            }
        }

        private RelayCommand<KeyRoutedEventArgs> _FocusPass;

        public RelayCommand<KeyRoutedEventArgs> FocusPass
        {
            get
            {
                return _FocusPass ?? (_FocusPass = new RelayCommand<KeyRoutedEventArgs>(async (KeyRoutedEventArgs args) =>
               {

                   if (args.Key == Windows.System.VirtualKey.Enter)
                   {
                       if (((Control)args.OriginalSource).Name == "tbLogin")
                       {
                           var pass = ((Panel)((Control)args.OriginalSource).Parent).Children.ToList().Where(x => x is PasswordBox).FirstOrDefault();
                           if (pass != null)
                           {
                               ((PasswordBox)pass).Focus(Windows.UI.Xaml.FocusState.Keyboard);
                           }
                       }
                       else
                       {
                           await Signin();
                       }
                   }

               }));

            }
        }

        #endregion

        #region Service

        private readonly IDataService _dataService;
        private readonly INavigationService _navigationService;
        private readonly IDialogService _dialogService;

        #endregion

        #region Properties

        private bool _isActive = true;

        public bool IsActive
        {
            get
            {
                return _isActive;
            }
            set
            {
                Set(ref _isActive, value);
            }
        }

        private string _txtLogin;

        public string TxtLogin
        {
            get
            {
                return _txtLogin;
            }
            set
            {
                Set(ref _txtLogin, value);
            }
        }

        private string _txtPassword;

        public string TxtPassword
        {
            get
            {
                return _txtPassword;
            }
            set
            {
                Set(ref _txtPassword, value);
            }
        }

        #endregion

        #region Ctor

        public LoginViewModel(
            IDataService dataService,
            INavigationService navigationService,
            IDialogService dialogService)
        {
            _dataService = dataService;
            _navigationService = navigationService;
            _dialogService = dialogService;
        }

        #endregion
    }
}
