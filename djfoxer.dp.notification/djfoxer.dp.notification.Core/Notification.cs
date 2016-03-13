using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static djfoxer.dp.notification.Core.Enum;

namespace djfoxer.dp.notification.Core
{
    public class Notification
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string TargetUrl { get; set; }

        public string AddedDate { get; set; }

        public NotificationType TypeValue { get; set; }

        public NotificationStatus StatusValue { get; set; }

        public int? TypeValueInt { get { return (int?)TypeValue; } }

        //public bool IconUrlVisibility { get { return (TypeValue == NotificationType.Comment || TypeValue == NotificationType.CommentBlog); } }

        public string MiddleTitle { get; set; }

        public string AdditionalInfo { get; set; }

        public string Icon { get; set; }
    }
}
