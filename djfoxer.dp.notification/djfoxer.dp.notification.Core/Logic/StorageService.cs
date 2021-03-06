﻿using djfoxer.dp.notification.Core;
using djfoxer.dp.notification.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace djfoxer.dp.notification.Core.Logic
{
    public class StorageService : BaseStorageService, IStorageService
    {
        #region TODO-TEMP

        private User CurrentUser;
        private Settings Settings;

        #endregion

        public User GetUser()
        {
            return CurrentUser;
        }




        public void WriteUser(string login, DateTime? expire)
        {
            CurrentUser = new User() { Login = login };
            Settings = new Settings() { LastUserName = login, LoginDate = DateTime.Now, Expire = expire ?? DateTime.Now };
            StorageLocalWrite(GetHashString(login), CurrentUser);
            StorageLocalWrite(Const.FileSettings, Settings);

        }



        public List<Notification> SaveNotifications(List<Notification> notifications)
        {
            List<Notification> freshNotifications = notifications.Where(x => x.Status == Core.Enum.NotificationStatus.New).ToList();
            notifications = notifications ?? new List<Notification>();
            if (CurrentUser != null)
            {
                if (CurrentUser.NewNotyifications != null && notifications != null)
                {
                    freshNotifications = freshNotifications.Where(x => !CurrentUser.NewNotyifications.Contains(x.Id)).ToList();
                }

                CurrentUser.NewNotyifications = notifications
                    .Where(x => x.Status == Core.Enum.NotificationStatus.New)
                    .Select(x => x.Id).ToList();
                CurrentUser.OldNotyifications = notifications
                    .Where(x => x.Status == Core.Enum.NotificationStatus.Old)
                    .Select(x => x.Id).ToList();

                StorageLocalWrite(GetHashString(CurrentUser.Login), CurrentUser);

            }

            return freshNotifications;
        }

        public bool LoadLastUser()
        {
            Settings = StorageLocalRead<Settings>(Const.FileSettings);
            if (Settings != null && !string.IsNullOrWhiteSpace(Settings.LastUserName))
            {

                if (Settings.Expire > DateTime.Now)
                {
                    CurrentUser = StorageLocalRead<User>(GetHashString(Settings.LastUserName));
                    return true;
                }
                else
                {
                    CurrentUser = null;
                    Settings = null;
                }
            }

            return false;
        }

        public void ClearData()
        {
            if (!string.IsNullOrEmpty(Settings.LastUserName))
            {
                StorageLocalDelete(GetHashString(Settings.LastUserName));
            }
            CurrentUser = null;
            Settings = null;
            StorageLocalDelete(Const.FileSettings);

        }
    }
}
