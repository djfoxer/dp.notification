﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace djfoxer.dp.notification.Core
{
    public class dpLogic
    {
        public async Task<string> GetSessionCookie(string login, string password)
        {
            WebResponse response = null;
            WebRequest request = null;
            string cookie = string.Empty;

            //get Cookie from old login page
            request = (HttpWebRequest)WebRequest.Create(Const.OldLoginUrlWithTimeStamp);
            request.Method = "GET";
            response = await request.GetResponseAsync();

            cookie = response.Headers["Set-cookie"].Split(';').FirstOrDefault();

            //login
            request = (HttpWebRequest)WebRequest.Create(Const.LoginUrl);
            request.Method = "POST";
            request.Headers["Cookie"] = cookie;
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            byte[] bytes = Encoding.UTF8.GetBytes(
                "what=login&login=" + Uri.EscapeDataString(login)
                + "&password=" + Uri.EscapeDataString(password) +
                "&persistent=true");
            using (Stream os = await request.GetRequestStreamAsync())
            {
                os.Write(bytes, 0, bytes.Length);
            }
            response = await request.GetResponseAsync();

            return cookie;
        }

        public async Task<List<Notification>> GetNotifications(string cookie)
        {
            WebResponse response = null;
            WebRequest request = null;

            request = (HttpWebRequest)WebRequest.Create(Const.NotifyUrlWithTimeStamp);
            request.Headers["Cookie"] = cookie;

            response = await request.GetResponseAsync();

            string pageSource;
            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
            {
                pageSource = sr.ReadToEnd();
            }

            var respList = (JObject)JsonConvert.DeserializeObject(pageSource);
            List<Notification> notList = new List<Notification>();

            if (respList.HasValues)
            {
                var c = respList.First.First;

                for (int i = 0; i < c.Count(); i++)
                {
                    var ele = (JProperty)c.ElementAt(i);
                    string json = ele.Value.ToString();
                    Notification n = JsonConvert.DeserializeObject<Notification>(json);
                    n.AddedDate = new DateTime(1970, 1, 1).AddMilliseconds((long)(((JValue)ele.Value["Data"]).Value));
                    n.TypeValue = Enum.ParseToNotificationType(((JValue)ele.Value["Type"]).Value.ToString());
                    n.PublicationId = ele.Name.Split(':')[0];
                    n.Id = ele.Name.Split(':')[1];
                    n.StatusValue = Enum.ParseToNotificationStatus((long)((JValue)ele.Value["Status"]).Value);
                    notList.Add(n);
                }
            }
            notList = notList.OrderBy(n => n.StatusValue).ThenByDescending(n => n.AddedDate).ToList();

            return notList;
        }

        private async Task<bool> ChangeStatusNotify(string id, string cookie, string method)
        {
            var req = (HttpWebRequest)WebRequest.Create(Const.NotifyUrlWithTimeStamp);
            req.Headers["Cookie"] = cookie;
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";

            byte[] bytes = Encoding.UTF8.GetBytes(string.Format("{1}%5B%5D={0}", id, method));
            using (Stream os = await req.GetRequestStreamAsync())
            {
                os.Write(bytes, 0, bytes.Length);
            }
            var resp = await req.GetResponseAsync();

            return true;
        }

        public async Task<bool> ReaddNotify(string id, string cookie)
        {
            return await ChangeStatusNotify(id, cookie, "markAsRead");
        }

        public async Task<bool> DeleteNotify(string id, string cookie)
        {
            return await ChangeStatusNotify(id, cookie, "deleteNotify");
        }

    }
}
