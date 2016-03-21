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
            switch (typeString)
            {
                case "comment":
                    return NotificationType.Comment;
                case "comment_blog":
                    return NotificationType.CommentBlog;
                case "program_update":
                    return NotificationType.ProgramUpdate;
                case "contest":
                    return NotificationType.Contest;
                case "friends_accept":
                    return NotificationType.FriendsAccept;
                case "friends_invite":
                    return NotificationType.FriendsInvite;
                case "blog_annotation":
                    return NotificationType.BlogAnnotation;
                case "private_msg":
                    return NotificationType.PrivateMsg;
                case "mention":
                    return NotificationType.Mention;
                case "license":
                    return NotificationType.License;
                case "badges":
                    return NotificationType.Badges;
                default:
                    return NotificationType.Unknown;
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
