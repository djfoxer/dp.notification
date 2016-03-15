using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using djfoxer.dp.notification.Core;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace djfoxer.dp.notification.Test
{
    [TestClass]
    public class UnitTest1
    {
        private static TestContext _testContext;

        [ClassInitialize]
        public static void SetupTests(TestContext testContext)
        {
            _testContext = testContext;
        }

        [TestMethod]
        public void GetNotification()
        {



            dpLogic logic = new dpLogic();
            string cookie = string.Empty;
            Task.Run(async () =>
            {
                cookie = await logic.GetSessionCookie(
                    _testContext.Properties["dpTestLogin"].ToString(),
                    _testContext.Properties["dpTestPassword"].ToString()
                    );
            }).GetAwaiter().GetResult();

            Assert.AreNotEqual(cookie, string.Empty);
            Assert.AreNotEqual(cookie, null);

            List<Notification> not = null;
            Task.Run(async () =>
            {
                not = await logic.GetNotifications(
                    cookie
                    );
            }).GetAwaiter().GetResult();


        }
    }
}
