using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace djfoxer.dp.notification.Core.Model
{
    public class Post
    {
        public string Id { get; set; }

        public int CommentsCounter { get; set; }

        public int VisitorsCounter { get; set; }

        public string Url { get; set; }

        public string Title { get; set; }

        public bool IsHomePage { get; set; }

        public bool IsPublished { get; set; }

        public int OrderId { get; set; }

        public DateTime DateLastModification { get; set; }
    }
}
