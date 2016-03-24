using djfoxer.dp.notification.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace djfoxer.dp.notification.App.Model
{

    public interface IDataService
    {
        Task<bool> Login(string login, string password);

        Task<List<Notification>> GetNotifications();
    }
}
