using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace djfoxer.dp.notification.Core
{
    public static class Const
    {
        private static string _OldLoginUrl = "https://ssl.dobreprogramy.pl/Logowanie.html";

        public static string OldLoginUrlWithTimeStamp
        {
            get
            {
                return _OldLoginUrl + "?v=" + GetCurrentDateInJsTimeStamp;
            }
        }

        private static string NotifyUrl = "http://www.dobreprogramy.pl/Providers/NotifyHelper.ashx";

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

        private static string _LoginUrl = "https://ssl.dobreprogramy.pl/Providers/LoginProvider.ashx";

        public static string LoginUrl
        {
            get
            {
                return _LoginUrl + "?v=" + GetCurrentDateInJsTimeStamp;
            }
        }

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
