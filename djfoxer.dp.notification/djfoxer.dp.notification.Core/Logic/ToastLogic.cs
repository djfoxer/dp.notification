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
        public void ShowToast(Notification notification, bool isForeground)
        {
            if (notification != null)
            {

                ToastContent toastContent = new ToastContent()
                {
                    Visual = new ToastVisual()
                    {
                        TitleText = new ToastText()
                        {
                            Text = notification.Title
                        },
                        BodyTextLine1 = new ToastText()
                        {
                            Text = notification.AddedDate.
                                    ToString("dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture)
                        },
                        BodyTextLine2 = new ToastText()
                        {
                            Text = notification.FullText
                        },
                        AppLogoOverride = new ToastAppLogo()
                        {
                            Source = new ToastImageSource(notification.Avatar)
                        }
                    },
                    Audio = new ToastAudio()
                    {
                        Src = new Uri("ms-appx:///Assets/Sounds/alert_1.mp3")
                    }
                };

                if (isForeground)
                {
                    toastContent.Actions = new ToastActionsCustom()
                    {
                        Buttons =
                        {
                            new ToastButton("pokaż", "show")
                            {
                                ActivationType = ToastActivationType.Foreground,
                                TextBoxId = notification.TargetUrl

                            },
                            new ToastButton("anuluj", "hide")
                            {
                                ActivationType = ToastActivationType.Foreground
                            },
                        }
                    };
                }

                ToastNotification toast = new ToastNotification(toastContent.GetXml());
                toast.Activated += Toast_Activated;

                ToastNotificationManager.CreateToastNotifier().Show(toast);
            }
        }

        private async void Toast_Activated(ToastNotification sender, object args)
        {
            if ((args as ToastActivatedEventArgs).Arguments == "show")
            {
                string url = ((XmlElement)sender.Content.GetElementsByTagName("action").First())
                            .GetAttribute("hint-inputId");
                if (!string.IsNullOrEmpty(url))
                {
                    await Launcher.LaunchUriAsync(new Uri(url));
                }
            }
        }
    }
}
