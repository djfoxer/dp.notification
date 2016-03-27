using djfoxer.dp.notification.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            var notifications = await _dpLogic.GetNotifications(user.Cookie);

            return notifications;
        }

        public async Task<bool> Login(string login, string password)
        {
            try
            {
                _dpLogic.DeleteSessionCookie();
                string cookie = await _dpLogic.SetSessionCookie(login, password);

                if (!string.IsNullOrWhiteSpace(cookie))
                {
                    _storageService.SaveUser(login, cookie);
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }


    }
}
