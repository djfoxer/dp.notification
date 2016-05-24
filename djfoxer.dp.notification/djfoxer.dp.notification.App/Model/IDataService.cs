using djfoxer.dp.notification.Core;
using djfoxer.dp.notification.Core.Model;
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

        void RefreshData();

        Task<List<Notification>> GetNotifications();

        List<Notification> SaveNotifications(List<Notification> notifications);

        Task<List<Notification>> SetNotificationAsOld(string notificationId);

        Task<List<Notification>> RemoveNotyfication(string notificationId);

        Task<BlogStatistic> GetFullBlogStatistics();
    }
}
