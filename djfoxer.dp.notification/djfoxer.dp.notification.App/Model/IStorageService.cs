using djfoxer.dp.notification.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace djfoxer.dp.notification.App.Model
{
    public interface IStorageService
    {
        void WriteUser(string login, DateTime? expire);

        Task<bool> LoadLastUser();

        User GetUser();

        List<Notification> SaveNotifications(List<Notification> notifications);

        void ClearData();

    }
}
