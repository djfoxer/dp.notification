using djfoxer.dp.notification.Core.Model;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace djfoxer.dp.notification.Core.Logic
{
    public class DpLogic
    {

        public async Task<Tuple<bool, DateTime?>> SetSessionCookie(string login, string password)
        {
            HttpResponseMessage response = null;
            HttpRequestMessage request = null;

            using (var httpClient = new HttpClient())
            {
                request = new HttpRequestMessage(HttpMethod.Post, new Uri(Const.UrlLogin));
                request.Content = new HttpFormUrlEncodedContent(new[] {
                    new KeyValuePair<string, string>("what", "login"),
                    new KeyValuePair<string, string>("login", login),
                    new KeyValuePair<string, string>("password", password),
                    new KeyValuePair<string, string>("persistent", "true"),
                });

                try
                {
                    response = await httpClient.SendRequestAsync(request);
                }
                catch (Exception)
                {
                    return new Tuple<bool, DateTime?>(false, null);
                }
            }

            var httpFilter = new Windows.Web.Http.Filters.HttpBaseProtocolFilter();
            var expireDate = httpFilter.CookieManager.GetCookies(new Uri(Const.UrlFullAddress)).First().Expires ?? DateTime.Now;

            return new Tuple<bool, DateTime?>(response.StatusCode == Windows.Web.Http.HttpStatusCode.Ok, expireDate.DateTime);
        }

        public void DeleteSessionCookie()
        {
            var httpFilter = new Windows.Web.Http.Filters.HttpBaseProtocolFilter();

            httpFilter.CookieManager.DeleteCookie(new HttpCookie(Const.CookieSessionName, Const.UrlDomain, "/"));
            httpFilter.CookieManager.DeleteCookie(new HttpCookie(Const.CookieSessionName2, Const.UrlDomain, "/"));

            httpFilter.CookieManager.DeleteCookie(new HttpCookie(Const.CookieSessionName, Const.UrlDomain2, "/"));
            httpFilter.CookieManager.DeleteCookie(new HttpCookie(Const.CookieSessionName2, Const.UrlDomain2, "/"));

        }

        public async Task<List<Notification>> GetNotifications()
        {
            HttpResponseMessage response = null;
            HttpRequestMessage request = null;

            using (var httpClient = new HttpClient())
            {
                request = new HttpRequestMessage(HttpMethod.Get, new Uri(Const.UrlNotifyWithTimeStamp));
                response = await httpClient.SendRequestAsync(request);
            }

            var respList = (JObject)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());

            List<Notification> notList = new List<Notification>();

            if (respList.HasValues)
            {
                var c = respList.First.First;

                for (int i = 0; i < c.Count(); i++)
                {
                    var ele = (JProperty)c.ElementAt(i);
                    Notification n = JsonConvert.DeserializeObject<Notification>(ele.Value.ToString());

                    n.AddedDate = new DateTime(1970, 1, 1).AddMilliseconds((long)(((JValue)ele.Value["Data"]).Value));
                    n.PublicationId = ele.Name.Split(':')[0];
                    n.Id = ele.Name.Split(':')[1];
                    n.TypeValue = Enum.ParseToNotificationType(((JValue)ele.Value["Type"]).Value.ToString());
                    notList.Add(n);
                }
            }
            notList = notList.OrderBy(n => n.Status).ThenByDescending(n => n.AddedDate).ToList();

            return notList;
        }

        private async Task<bool> ChangeStatusNotify(string id, string method)
        {

            HttpResponseMessage response = null;
            HttpRequestMessage request = null;

            using (var httpClient = new HttpClient())
            {
                request = new HttpRequestMessage(HttpMethod.Post, new Uri(Const.UrlNotifyRaw));
                request.Content = new HttpFormUrlEncodedContent(new[] {
                    new KeyValuePair<string, string>(method+"[]", id)
                });

                response = await httpClient.SendRequestAsync(request);
            }
            return response.StatusCode == Windows.Web.Http.HttpStatusCode.Ok;
        }

        public async Task<bool> ReadNotify(string id)
        {
            return await ChangeStatusNotify(id, "markAsRead");
        }

        public async Task<bool> DeleteNotify(string id)
        {
            return await ChangeStatusNotify(id, "deleteNotify");
        }

        public async Task<BlogStatistic> GetFullBlogStatistic()
        {
            BlogStatistic stat = new BlogStatistic();
            stat.Posts = new List<Post>();

            try
            {
                using (var httpClient = new HttpClient())
                {
                    await GetBlogMainStatistics(1, stat.Posts, httpClient);
                    await GetBlogCounters(stat.Posts, httpClient);
                }
                stat.RefreshMainStatistics();
            }
            catch (Exception)
            {
            }

            return stat;
        }

public async Task<List<Post>> GetBlogCounters(List<Post> postLink, HttpClient httpClient)
{
    HtmlDocument doc = new HtmlDocument();

    foreach (var post in postLink)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, new Uri(post.Url));
        var response = await httpClient.SendRequestAsync(request);

        doc.LoadHtml(await response.Content.ReadAsStringAsync());

        var details = doc.DocumentNode.Descendants("section")
            .Where(d => d.Attributes.Contains("class") &&
            d.Attributes["class"].Value.Contains("user-info")).LastOrDefault();
        if (details != null)
        {
            var divs = details.Descendants("div").ToList();
            if (divs.Count >= 12)
            {
                post.VisitorsCounter = int.Parse(divs[9].InnerText);
                post.CommentsCounter = int.Parse(divs[12].InnerText);
                post.DateLastModification = DateTime.ParseExact(divs[5].InnerText, 
                    "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture);
            }
        }
    }

    return postLink;
}

        public async Task<List<Post>> GetBlogMainStatistics(int pageNo, List<Post> postLink, HttpClient httpClient)
        {

            var request = new HttpRequestMessage(HttpMethod.Get, new Uri(Const.BlogPrefix + pageNo + ".html"));
            var response = await httpClient.SendRequestAsync(request);

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(await response.Content.ReadAsStringAsync());

            var divWithLinks = doc.DocumentNode.Descendants("div")
                    .Where(d => d.Attributes.Contains("class") &&
                    d.Attributes["class"].Value.Contains("contentText"))
                    .FirstOrDefault();
            if (divWithLinks != null)
            {
                int lastOrderId = postLink.Select(x => x.OrderId).LastOrDefault();

                divWithLinks.Descendants("tr").ToList().ForEach(x =>
                {
                    var elemA = x.Descendants("a").FirstOrDefault();
                    var elemSpan = x.Descendants("span").FirstOrDefault();

                    if (elemA != null && elemSpan != null)
                    {
                        var newPost = new Post()
                        {
                            Title = elemA.InnerText,
                            Url = elemA.Attributes["href"].Value,
                            IsPublished = elemSpan.InnerText == Const.PostStatusPublished,
                            IsHomePage = elemSpan.Attributes.Contains("class") &&
                                elemSpan.Attributes["class"].Value.Contains(Const.PostHomePage),
                            OrderId = ++lastOrderId
                        };
                        newPost.Id = newPost.Url
                            .Split(new string[] { ",", ".html" }, StringSplitOptions.RemoveEmptyEntries)
                                .Reverse().First();
                        postLink.Add(newPost);
                    }


                });
            }

            var nextLink = doc.DocumentNode.Descendants("div")
                .Where(d => d.Attributes.Contains("class") &&
                    d.Attributes["class"].Value.Contains("controls"))
                    .FirstOrDefault();

            var nextUrl = (Const.BlogPrefix + (pageNo + 1) + ".html");

            if (nextLink != null && nextLink.Descendants("a").Where(a => a.Attributes.Contains("href") &&
            a.Attributes["href"].Value == nextUrl).Count() > 0)
            {
                await GetBlogMainStatistics((pageNo + 1), postLink, httpClient);
            }

            return postLink;
        }
    }
}
