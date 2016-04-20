using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.UI.Notifications;

namespace djfoxer.dp.notification.Background
{
    public sealed class BackgroundTaskManager
    {
        public async void Register()
        {
            var status = await BackgroundExecutionManager.RequestAccessAsync();
            if (!BackgroundTaskRegistration.AllTasks.Select(x => x.Value.Name).ToList().Exists(x => x == GetNotificationBackgroundTask.TaskName))
            {
                BackgroundTaskRegistration.AllTasks.Where(x => x.Value.Name.StartsWith(nameof(GetNotificationBackgroundTask))).ToList().ForEach(x => x.Value.Unregister(true));
                GetNotificationBackgroundTask.RegisterMe();
            }

        }
    }
}
