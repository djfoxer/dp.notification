using djfoxer.dp.notification.Core.Model;
using NotificationsExtensions.Toasts;
using System;
using System.Globalization;
using System.Linq;
using Windows.Data.Xml.Dom;
using Windows.System;
using Windows.UI.Notifications;

namespace djfoxer.dp.notification.Core.Logic
{
    public class ToastLogic
    {
        public void ShowToast(Notification notification)
        {
            if (notification != null)
            {

                //ToastActionsCustom actions = new ToastActionsCustom()
                //{
                //    Inputs
                //}

                XmlDocument toastXml = new XmlDocument();
                toastXml.LoadXml(
                    $@"
                    <toast>
                        <visual>
                            <binding template='ToastGeneric'>
                                <text></text>
                                <text></text>
                                <text></text>
                                <image placement='appLogoOverride'></image>
                            </binding>
                        </visual>
                        <audio src='ms-winsoundevent:Notification.Looping.Alarm2' loop='false' />
                        <actions>
                            <action
                                content='pokaż'
                                activationType='foreground'
                                arguments='show'/>

                            <action
                                content='anuluj'
                                activationType='foreground'
                                arguments='hide'/>
                        </actions>
                    </toast>"
                );

                // toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastImageAndText03);

                XmlNodeList toastImageAttributes = toastXml.GetElementsByTagName("image");
                ((XmlElement)toastImageAttributes[0]).SetAttribute("src", notification.Avatar);
                ((XmlElement)toastImageAttributes[0]).SetAttribute("alt", "img");

                XmlNodeList toastTextAttributes = toastXml.GetElementsByTagName("text");
                toastTextAttributes[0].InnerText = notification.Title;
                toastTextAttributes[1].InnerText = notification.AddedDate.
                                    ToString("dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture);
                toastTextAttributes[2].InnerText = notification.FullText;

                toastTextAttributes = toastXml.GetElementsByTagName("actions");
                ((XmlElement)toastTextAttributes[0]).SetAttribute("url", notification.TargetUrl);

                ToastNotification toast = new ToastNotification(toastXml);
                toast.Activated += Toast_Activated;

                ToastNotificationManager.CreateToastNotifier().Show(toast);
            }
        }

        private async void Toast_Activated(ToastNotification sender, object args)
        {
            if ((args as ToastActivatedEventArgs).Arguments == "show")
            {
                string url = ((XmlElement)sender.Content.GetElementsByTagName("actions").First())
                            .GetAttribute("url");
                if (!string.IsNullOrEmpty(url))
                {
                    await Launcher.LaunchUriAsync(new Uri(url));
                }
            }
        }
    }
}
