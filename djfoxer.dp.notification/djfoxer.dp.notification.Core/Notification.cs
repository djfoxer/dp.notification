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

        public string PublicationId { get; set; }

        public string Avatar { get; set; }

        public string CustomText { get; set; }

        public string Title { get; set; }

        public string TargetUrl { get; set; }

        public DateTime AddedDate { get; set; }

        public NotificationType TypeValue { get; set; }

        public NotificationStatus Status { get; set; }

        public string UserName { get; set; }
    }
}
