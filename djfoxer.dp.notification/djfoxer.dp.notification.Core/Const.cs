using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace djfoxer.dp.notification.Core
{
    public static class Const
    {
        private const string _UrlToGetCookie = "http://www.dobreprogramy.pl/";

        public static string UrlToGetCookie
        {
            get
            {
                return _UrlToGetCookie + "?v=" + GetCurrentDateInJsTimeStamp;
            }
        }

        private const string NotifyUrl = "http://www.dobreprogramy.pl/Providers/NotifyHelper.ashx";

        public static string NotifyUrlWithTimeStamp
        {
            get
            {
                return NotifyUrl + "?ping=ping&_=" + GetCurrentDateInJsTimeStamp;
            }
        }

        public static string NotifyUrlRaw
        {
            get
            {
                return NotifyUrl;
            }
        }

        private const string _LoginUrl = "https://ssl.dobreprogramy.pl/Providers/LoginProvider.ashx";

        public static string LoginUrl
        {
            get
            {
                return _LoginUrl + "?v=" + GetCurrentDateInJsTimeStamp;
            }
        }

        public const string RequestContentType = "application/x-www-form-urlencoded; charset=UTF-8";

        public static double GetCurrentDateInJsTimeStamp
        {
            get
            {
                return
                Math.Round(DateTime.UtcNow
                .Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc))
                .TotalMilliseconds, 0);
            }
        }
    }
}
