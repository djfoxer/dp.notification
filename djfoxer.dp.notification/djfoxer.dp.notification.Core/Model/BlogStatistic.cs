using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace djfoxer.dp.notification.Core.Model
{
    public class BlogStatistic
    {
        public List<Post> Posts { get; set; }

        public int PostsCounter { get; set; }

        public int VisitorsCounter { get; set; }

        public int CommentsCounter { get; set; }

        public int PostHomePageCounter { get; set; }

        public void RefreshMainStatistics()
        {
            PostsCounter = VisitorsCounter = CommentsCounter = PostHomePageCounter = 0;
            if (Posts != null)
            {
                foreach (var post in Posts)
                {
                    PostsCounter++;
                    VisitorsCounter += post.VisitorsCounter;
                    CommentsCounter += post.CommentsCounter;
                    if (post.IsHomePage)
                    {
                        PostHomePageCounter++;
                    }
                }
            }
        }

        public BlogStatistic Sort(int index)
        {
            if (Posts != null)
            {
                Posts = (index == 0 ? Posts.OrderByDescending(x => x.VisitorsCounter) : (index == 1 ? Posts.OrderByDescending(x => x.CommentsCounter) : Posts.OrderBy(x => x.OrderId))).ToList();
            }
            return this;
        }

        public BlogStatistic Clear()
        {
            Posts = new List<Post>();
            PostsCounter = VisitorsCounter = CommentsCounter = PostHomePageCounter = 0;

            return this;
        }
    }
}
