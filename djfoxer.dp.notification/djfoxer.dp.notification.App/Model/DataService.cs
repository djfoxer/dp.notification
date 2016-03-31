using djfoxer.dp.notification.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.System;
using Windows.UI.Notifications;

namespace djfoxer.dp.notification.App.Model
{
    public class DataService : IDataService
    {

        #region Prop


        #endregion

        #region Logic

        private readonly DpLogic _dpLogic;
        private readonly IStorageService _storageService;

        #endregion

        public DataService(IStorageService storageService)
        {
            _dpLogic = new DpLogic();
            _storageService = storageService;
        }

        public async Task<List<Notification>> GetNotifications()
        {
            var user = _storageService.GetUser();
            if (user == null)
            {
                return null;
            }
            var notifications = await _dpLogic.GetNotifications();

            return notifications;
        }

        public async Task<bool> Login(string login, string password)
        {
            try
            {
                _dpLogic.DeleteSessionCookie();
                bool success = await _dpLogic.SetSessionCookie(login, password);

                if (success)
                {
                    _storageService.SaveUser(login);
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void ShowToast(Notification notification)
        {
            if (notification != null)
            {
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
                                    ToString("dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture);
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
