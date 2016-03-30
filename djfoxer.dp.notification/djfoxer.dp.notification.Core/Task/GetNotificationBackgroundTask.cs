using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.UI.Notifications;

namespace djfoxer.dp.notification.Core.Task
{
    public sealed class GetNotificationBackgroundTask : MainBackgroundTask
    {


        public override void Run(IBackgroundTaskInstance taskInstance)
        {
            BackgroundTaskDeferral _deferral = taskInstance.GetDeferral();

            var xmlString = @"<toast launch='args' scenario='alarm'>
    <visual>
        <binding template='ToastGeneric'>
            <text>Alarm</text>
            <text>Get up now!!</text>
        </binding>
    </visual>
    <actions>

        <action arguments = 'snooze'
                content = 'snooze' />

        <action arguments = 'dismiss'
                content = 'dismiss' />

    </actions>
</toast>";
            var doc = new Windows.Data.Xml.Dom.XmlDocument();
            doc.LoadXml(xmlString);

            var toast = new ToastNotification(doc);
            ToastNotificationManager.CreateToastNotifier().Show(toast);

            _deferral.Complete();
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
            builder.SetTrigger(new TimeTrigger(15, false));
            builder.TaskEntryPoint = "djfoxer.dp.notification.Core.Task." + TaskName;
            builder.Register();
        }
    }
}
