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

        #region Logic

        private readonly DpLogic _dpLogic;

        #endregion

        public DataService()
        {
            _dpLogic = new DpLogic();
        }

        public async Task<bool> Login(string login, string password)
        {
            try
            {
                string cookie = await _dpLogic.GetSessionCookie(login, password);

                return !string.IsNullOrWhiteSpace(cookie);
            }
            catch (Exception)
            {
                return false;
            }



        }
    }
}
