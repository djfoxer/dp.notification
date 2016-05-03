using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using static djfoxer.dp.notification.Core.Enum;

namespace djfoxer.dp.notification.Core.Model
{
    public class Notification
    {
        public string Id { get; set; }

        public string PublicationId { get; set; }

        public string Avatar { get; set; }

        public string CustomText { get; set; }

        public string Title { get; set; }

        public string TargetUrl { get; set; }

        public Symbol? Icon { get; set; }

        public DateTime AddedDate { get; set; }

        private NotificationType _TypeValue = NotificationType.Unknown;
        public NotificationType TypeValue
        {
            get { return _TypeValue; }
            set
            {
                _TypeValue = value;
                switch (value)
                {
                    case NotificationType.Comment:
                        Title = "Nowy komentarz";
                        CustomText = "odpowiedział(a) na Twój komentarz";
                        FixAvatarUrl();
                        break;
                    case NotificationType.CommentBlog:
                        CustomText = "skomentował(a) Twój wpis";
                        FixAvatarUrl();
                        break;
                    case NotificationType.ProgramUpdate:
                        CustomText = "Program, który masz w ulubionych został zaktualizowany";
                        FixAvatarUrl("cloud-up.png");
                        break;
                    case NotificationType.Contest:
                        FixAvatarUrl("gift.png");
                        break;
                    case NotificationType.FriendsAccept:
                        Title = "Zaproszenie zaakceptowane";
                        CustomText = "zaakceptował Twoje zaproszenie do grona znajomych";
                        FixAvatarUrl("handshake.png");
                        break;
                    case NotificationType.FriendsInvite:
                        Title = "Zaproszenie do znajomych";
                        CustomText = "zaprosił Ciebie do grona znajomych";
                        FixAvatarUrl("profile-group.png");
                        break;
                    case NotificationType.BlogAnnotation:
                    case NotificationType.PrivateMsg:
                    case NotificationType.License:
                        FixAvatarUrl();
                        break;
                    case NotificationType.Mention:
                        Title = "Użytkownik wspomniał o Tobie";
                        CustomText = "oznaczył Ciebie w komentarzu";
                        FixAvatarUrl();
                        break;
                    case NotificationType.Badges:
                        CustomText = "Nowa odznaka!";
                        FixAvatarUrl("flower.png");
                        break;
                    case NotificationType.CommentVote:
                        Title = "Polubiono Twoją wypowiedź";
                        CustomText = "oddał głos na Twój komentarz";
                        FixAvatarUrl();
                        break;
                    case NotificationType.CommentFollow:
                        Title = "Polubiono Twoją wypowiedź";
                        CustomText = "skomentował wpis, który dodano do ulubionych";
                        FixAvatarUrl();
                        break;
                    default:
                        FixAvatarUrl();
                        break;
                }

            }
        }

        public NotificationStatus Status { get; set; }

        public string UserName { get; set; }

        public string FullText
        {
            get
            {
                return UserName + (string.IsNullOrEmpty(UserName) ? CustomText : " " + CustomText);
            }
        }

        private void FixAvatarUrl(string icoInput = null)
        {
            if (!string.IsNullOrWhiteSpace(Avatar) && !Avatar.EndsWith("svg"))
            {
                if (!Avatar.StartsWith("http"))
                {
                    Avatar = "http:" + Avatar;
                }
            }
            else
            {
                Avatar = "file:///" + Path.GetFullPath("Assets/NotificationIco/" + (string.IsNullOrEmpty(icoInput) ? "star.png" : icoInput));
            }
        }
    }
}
