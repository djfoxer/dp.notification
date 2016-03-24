using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace djfoxer.dp.notification.Core
{
    public class DpLogic
    {

        public async Task<string> GetSessionCookie(string login, string password)
        {
            HttpResponseMessage response = null;
            HttpRequestMessage request = null;
            string cookie = null;

            var httpFilter = new Windows.Web.Http.Filters.HttpBaseProtocolFilter();
            httpFilter.CookieUsageBehavior = Windows.Web.Http.Filters.HttpCookieUsageBehavior.NoCookies;

            using (var httpClient = new HttpClient(httpFilter))
            {
                response = await httpClient.GetAsync(new Uri(Const.UrlToGetCookie));
                cookie = response.Headers["Set-cookie"]?.Split(';')?.FirstOrDefault();
            }

            httpFilter = new Windows.Web.Http.Filters.HttpBaseProtocolFilter();
            httpFilter.CookieUsageBehavior = Windows.Web.Http.Filters.HttpCookieUsageBehavior.Default;

            using (var httpClient = new HttpClient(httpFilter))
            {
                request = new HttpRequestMessage(HttpMethod.Post, new Uri(Const.LoginUrl));
                request.Content = new HttpFormUrlEncodedContent(new[] {
                new KeyValuePair<string, string>("what", "login"),
                new KeyValuePair<string, string>("login", login),
                new KeyValuePair<string, string>("password", password),
                new KeyValuePair<string, string>("persistent", "true"),
                    });
                request.Headers["Cookie"] = cookie;

                try
                {
                    response = await httpClient.SendRequestAsync(request);
                }
                catch (Exception)
                {
                    return null;
                }
            }
        
            return response.StatusCode == Windows.Web.Http.HttpStatusCode.Ok? cookie : null;
        }

    public async Task<List<Notification>> GetNotifications(string cookie)
    {
        HttpResponseMessage response = null;
        HttpRequestMessage request = null;

        //var httpFilter = new Windows.Web.Http.Filters.HttpBaseProtocolFilter();
        //httpFilter.CookieUsageBehavior = Windows.Web.Http.Filters.HttpCookieUsageBehavior.Default;

        using (var httpClient = new HttpClient())
        {

            request = new HttpRequestMessage(HttpMethod.Get, new Uri(Const.NotifyUrlWithTimeStamp));
            request.Headers["Cookie"] = cookie;

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
                n.TypeValue = Enum.ParseToNotificationType(((JValue)ele.Value["Type"]).Value.ToString());
                n.PublicationId = ele.Name.Split(':')[0];
                n.Id = ele.Name.Split(':')[1];
                notList.Add(n);
            }
        }
        notList = notList.OrderBy(n => n.Status).ThenByDescending(n => n.AddedDate).ToList();

        return notList;
    }

    private async Task<bool> ChangeStatusNotify(string id, string cookie, string method)
    {

        HttpResponseMessage response = null;
        HttpRequestMessage request = null;

        using (var httpClient = new HttpClient())
        {

            request = new HttpRequestMessage(HttpMethod.Post, new Uri(Const.NotifyUrlRaw));
            request.Headers["Cookie"] = cookie;
            request.Content = new HttpFormUrlEncodedContent(new[] {
                new KeyValuePair<string, string>(method+"[]", id)
            }); ;

            response = await httpClient.SendRequestAsync(request);
        }
        return response.StatusCode == Windows.Web.Http.HttpStatusCode.Ok;
    }

    public async Task<bool> ReadNotify(string id, string cookie)
    {
        return await ChangeStatusNotify(id, cookie, "markAsRead");
    }

    public async Task<bool> DeleteNotify(string id, string cookie)
    {
        return await ChangeStatusNotify(id, cookie, "deleteNotify");
    }

}
}
