using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace djfoxer.dp.notification.App.Model
{
    public class StorageService : IStorageService
    {
        #region TODO-TEMP

        //CHANGE!!!!!
        private static User CurrentUser;

        #endregion

        public User GetUser()
        {
            return CurrentUser;
        }

      
        public void SaveUser(string login)
        {
            CurrentUser = new User() { Login = login, LoginDate = DateTime.Now };
        }

  
    }
}
