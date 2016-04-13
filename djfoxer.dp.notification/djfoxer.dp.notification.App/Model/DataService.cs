using djfoxer.dp.notification.Core;
using djfoxer.dp.notification.Core.Logic;
using djfoxer.dp.notification.Core.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace djfoxer.dp.notification.App.Model
{
    public class DataService : IDataService
    {

        #region Prop


        #endregion

        #region Logic

        private readonly DpLogic _dpLogic;
        private readonly IStorageService _storageService;

        #endregion

        public DataService(IStorageService storageService)
        {
            _dpLogic = new DpLogic();
            _storageService = storageService;
        }

        public async Task<List<Notification>> GetNotifications()
        {
            var user = _storageService.GetUser();
            if (user == null)
            {
                return null;
            }
            var notifications = await _dpLogic.GetNotifications();

            return notifications;
        }

        public async Task<bool> Login(string login, string password)
        {
            try
            {
                _dpLogic.DeleteSessionCookie();
                Tuple<bool, DateTime?> loginValue = await _dpLogic.SetSessionCookie(login, password);

                if (loginValue.Item1)
                {
                    _storageService.WriteUser(login, loginValue.Item2);
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void Logout()
        {
            try
            {
                _dpLogic.DeleteSessionCookie();
                _storageService.ClearData();
            }
            catch (Exception)
            {
            }
        }

        public List<Notification> SaveNotifications(List<Notification> notifications)
        {
            return _storageService.SaveNotifications(notifications);
        }

        public async void SetNotificationAsOld(string notificationId)
        {
            var notifications = await GetNotifications();
            if (notifications != null)
            {
                notifications.ForEach(x => { if (x.Id == notificationId) { x.Status = Core.Enum.NotificationStatus.Old; } });
                SaveNotifications(notifications);
                await _dpLogic.ReadNotify(notificationId);
            }
        }

        
    }
}
