using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using djfoxer.dp.notification.Core;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

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



            DpLogic logic = new DpLogic();
            string cookie = string.Empty;

            logic.DeleteSessionCookie();

            Task.Run(async () =>
            {
                cookie = await logic.SetSessionCookie(
                    _testContext.Properties["dpTestLogin"].ToString(),
                    _testContext.Properties["dpTestPassword"].ToString()
                    );
            }).GetAwaiter().GetResult();

            Assert.AreNotEqual(cookie, string.Empty);
            Assert.AreNotEqual(cookie, null);

            //OLD
            List<Notification> not = null;
            Task.Run(async () =>
            {
                not = await logic.GetNotifications(
                    cookie
                    );
            }).GetAwaiter().GetResult();

            //Assert.IsNotNull(not);

            //Task.Run(async () =>
            //{
            //    await logic.ReadNotify(not.FirstOrDefault().Id, cookie);
            //}).GetAwaiter().GetResult();
        }
    }
}
