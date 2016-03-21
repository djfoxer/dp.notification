﻿using djfoxer.dp.notification.App.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace djfoxer.dp.notification.App.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {

        #region Commands

        private RelayCommand _login;

        public RelayCommand Login
        {
            get
            {
                return _login ?? (_login = new RelayCommand(async () =>
                {
                    IsActive = false;
                    var loginResult = await _dataService.Login(TxtLogin, TxtPassword);

                    var dialog = ServiceLocator.Current.GetInstance<IDialogService>();
                    await dialog.ShowMessage(loginResult ? "Logowanie udane" : "Błędny login/hasło", "Logowanie");
                    IsActive = true;

                }));

            }
        }

        #endregion

        #region Service

        private readonly IDataService _dataService;

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
            IDataService dataService)
        {
            _dataService = dataService;
        }

        #endregion
    }
}