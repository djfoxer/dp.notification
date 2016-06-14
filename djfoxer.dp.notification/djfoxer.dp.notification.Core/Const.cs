using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace djfoxer.dp.notification.Core
{
    public static class Const
    {

        private const string _UrlDomain = "dobreprogramy.pl";

        public static string UrlDomain
        {
            get
            {
                return _UrlDomain;
            }
        }

        private const string _UrlRegister = "https://ssl.dobreprogramy.pl/Rejestracja.html";

        public static string UrlRegister
        {
            get
            {
                return _UrlRegister;
            }
        }

        private const string _UrlDomain3 = "http://www.dobreprogramy.pl/";

        public static string UrlFullAddress
        {
            get
            {
                return _UrlDomain3;
            }
        }

        private const string _BlogPrefix = "http://www.dobreprogramy.pl/MojBlog,";

        public static string BlogPrefix
        {
            get
            {
                return _BlogPrefix;
            }
        }

        private const string _PostStatusPublished = "opublikowany";

        public static string PostStatusPublished
        {
            get
            {
                return _PostStatusPublished;
            }
        }

        private const string _PostHomePage = "homepage";

        public static string PostHomePage
        {
            get
            {
                return _PostHomePage;
            }
        }

        private const string _UrlDomain2 = "www.dobreprogramy.pl";

        public static string UrlDomain2
        {
            get
            {
                return _UrlDomain2;
            }
        }

        private const string _CookieSessionName = "ASP.NET_SessionId";

        public static string CookieSessionName
        {
            get
            {
                return _CookieSessionName;
            }
        }

        private const string _CookieSessionName2 = "NGDP_Auth";

        public static string CookieSessionName2
        {
            get
            {
                return _CookieSessionName2;
            }
        }

        private const string UrlNotify = "http://www.dobreprogramy.pl/Providers/NotifyHelper.ashx";

        public static string UrlNotifyWithTimeStamp
        {
            get
            {
                return UrlNotify + "?ping=ping&_=" + GetCurrentDateInJsTimeStamp;
            }
        }

        public static string UrlNotifyRaw
        {
            get
            {
                return UrlNotify;
            }
        }

        private const string _UrlLogin = "https://ssl.dobreprogramy.pl/Providers/LoginProvider.ashx";

        public static string UrlLogin
        {
            get
            {
                return _UrlLogin + "?v=" + GetCurrentDateInJsTimeStamp;
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

        public const string FileSettings = "settings.json";
    }
}
