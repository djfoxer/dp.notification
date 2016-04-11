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

        void Logout();

        Task<List<Notification>> GetNotifications();

        List<Notification> SaveNotifications(List<Notification> notifications);


        void ShowToast(Notification notification);

        void SetNotificationAsOld(string notificationId);
    }
}
