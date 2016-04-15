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
            //System.Diagnostics.Debug.WriteLine("POSZŁO!!!!!!!!!");

            BackgroundTaskDeferral _deferral = taskInstance.GetDeferral();
            try
            {
                IStorageService storage = new StorageService();
                if (await storage.LoadLastUser())
                {
                    DpLogic dpLogic = new DpLogic();
                    ToastLogic toastLogic = new ToastLogic();

                    var notifications = await dpLogic.GetNotifications();

                    notifications = storage.SaveNotifications(notifications);

                    notifications.ForEach(x => toastLogic.ShowToast(x));

                    //StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync("439280567e180293388d226da13ae6fd");
                    //await FileIO.WriteTextAsync(file, "asdsad");


                }
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

            Type thisTask = typeof(GetNotificationBackgroundTask);

            var builder = new BackgroundTaskBuilder();
            builder.Name = thisTask.Name;
            builder.AddCondition(new SystemCondition(SystemConditionType.InternetAvailable));
            var timer = new TimeTrigger(15, false);
            builder.SetTrigger(timer);
            builder.TaskEntryPoint = thisTask.FullName;
            var task = builder.Register();
        }
    }
}
