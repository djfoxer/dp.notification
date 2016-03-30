using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.UI.Notifications;

namespace djfoxer.dp.notification.Core.Task
{
    public class BackgroundTaskManager
    {



        public void Register()
        {



            if (!BackgroundTaskRegistration.AllTasks.Select(x => x.Value.Name).ToList().Exists(x => x == GetNotificationBackgroundTask.TaskName))
            {
                GetNotificationBackgroundTask.RegisterMe();
            }

        }
    }
}
