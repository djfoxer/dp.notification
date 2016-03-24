using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace djfoxer.dp.notification.App.Model
{
    public interface IStorageService
    {
        void SaveUser(string login, string cookie);

        User GetUser();
    }
}
