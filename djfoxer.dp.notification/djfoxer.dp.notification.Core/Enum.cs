using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace djfoxer.dp.notification.Core
{
    public static class Enum
    {

        public static NotificationType ParseToNotificationType(string typeString)
        {
            if (string.IsNullOrWhiteSpace(typeString))
            {
                return NotificationType.Unknown;
            }

            typeString = typeString.ToLower();

            if (typeString == "comment")
            {
                return NotificationType.Comment;
            }
            else if (typeString == "comment_blog")
            {
                return NotificationType.CommentBlog;
            }
            else if (typeString == "program_update")
            {
                return NotificationType.ProgramUpdate;
            }
            else if (typeString == "contest")
            {
                return NotificationType.Contest;
            }
            else if (typeString == "friends_accept")
            {
                return NotificationType.FriendsAccept;
            }
            else if (typeString == "friends_invite")
            {
                return NotificationType.FriendsInvite;
            }
            else if (typeString == "blog_annotation")
            {
                return NotificationType.BlogAnnotation;
            }
            else if (typeString == "private_msg")
            {
                return NotificationType.PrivateMsg;
            }
            else if (typeString == "mention")
            {
                return NotificationType.Mention;
            }
            else if (typeString == "license")
            {
                return NotificationType.License;
            }
            else if (typeString == "badges")
            {
                return NotificationType.Badges;
            }
            else
            {
                return NotificationType.Unknown;
            }
        }

        public static NotificationStatus ParseToNotificationStatus(long status)
        {
            if (status == 0)
            {
                return NotificationStatus.New;
            }
            else if (status == 1)
            {
                return NotificationStatus.Old;
            }
            else
            {
                return NotificationStatus.Unknown;
            }
        }

        public enum NotificationType
        {
            Unknown = -1,
            Comment = 0,
            CommentBlog = 1,
            ProgramUpdate = 2,
            Contest = 3,
            FriendsAccept = 4,
            FriendsInvite = 5,
            BlogAnnotation = 6,
            PrivateMsg = 7,
            Mention = 8,
            License = 9,
            Badges = 10,
        }

        public enum NotificationStatus
        {
            Unknown = -1,
            New = 0,
            Old = 1
        }
    }
}
