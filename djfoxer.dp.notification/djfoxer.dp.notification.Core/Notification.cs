using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using static djfoxer.dp.notification.Core.Enum;

namespace djfoxer.dp.notification.Core
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
                        FixAvatarUrl(Symbol.OutlineStar);
                        break;
                    case NotificationType.Contest:
                        FixAvatarUrl(Symbol.Like);
                        break;
                    case NotificationType.FriendsAccept:
                        Title = "Zaproszenie zaakceptowane";
                        CustomText = "zaakceptował Twoje zaproszenie do grona znajomych";
                        FixAvatarUrl(Symbol.AddFriend);
                        break;
                    case NotificationType.FriendsInvite:
                        Title = "Zaproszenie do znajomych";
                        CustomText = "zaprosił Ciebie do grona znajomych";
                        FixAvatarUrl(Symbol.People);
                        break;
                    case NotificationType.BlogAnnotation:
                        break;
                    case NotificationType.PrivateMsg:
                        break;
                    case NotificationType.Mention:
                        break;
                    case NotificationType.License:
                        break;
                    case NotificationType.Badges:
                        CustomText = "Nowa odznaka!";
                        break;
                    default:
                        break;
                }
            }
        }

        public NotificationStatus Status { get; set; }

        public string UserName { get; set; }

        private void FixAvatarUrl(Symbol? symbol = null)
        {
            if (!string.IsNullOrWhiteSpace(Avatar) && !Avatar.StartsWith("http"))
            {
                Avatar = "http:" + Avatar;
            }
            else
            {
                if (symbol.HasValue)
                {
                    Icon = symbol.Value;
                }
            }
        }
    }
}
