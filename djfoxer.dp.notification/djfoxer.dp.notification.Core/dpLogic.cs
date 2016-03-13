using Newtonsoft.Json;
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

            dynamic not = JsonConvert.DeserializeObject(pageSource);

            return null;
        }

    }
}
