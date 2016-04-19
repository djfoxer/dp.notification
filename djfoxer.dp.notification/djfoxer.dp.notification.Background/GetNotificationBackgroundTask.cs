using djfoxer.dp.notification.Core;
using djfoxer.dp.notification.Core.Logic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.ApplicationModel.Background;
using Windows.Storage;
using Windows.UI.Notifications;

namespace djfoxer.dp.notification.Background
{
    public sealed class GetNotificationBackgroundTask : IBackgroundTask
    {


        public async void Run(IBackgroundTaskInstance taskInstance)
        {

            BackgroundTaskDeferral _deferral = taskInstance.GetDeferral();
            try
            {
                IStorageService storage = new StorageService();
                if (storage.LoadLastUser())
                {
                    DpLogic dpLogic = new DpLogic();
                    ToastLogic toastLogic = new ToastLogic();

                    var notifications = await dpLogic.GetNotifications();

                    //toastLogic.ShowToast(notifications.First());

                    notifications = storage.SaveNotifications(notifications);

                    notifications.ForEach(x => toastLogic.ShowToast(x));


                }
            }
            catch (Exception)
            {

            }
            finally
            {
                _deferral.Complete();
            }
        }

        private static string _TaskName = string.Empty;

        public static string TaskName
        {
            get
            {
                if (string.IsNullOrEmpty(_TaskName))
                {
                    _TaskName = nameof(GetNotificationBackgroundTask);
                }
                return _TaskName;
            }
        }

        public static void RegisterMe()
        {
            var builder = new BackgroundTaskBuilder();
            builder.Name = TaskName;
            builder.AddCondition(new SystemCondition(SystemConditionType.InternetAvailable));
            var timer = new TimeTrigger(15, false);
            builder.SetTrigger(timer);
            builder.TaskEntryPoint = typeof(GetNotificationBackgroundTask).FullName;
            var task = builder.Register();
        }
    }
}
