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
            bool success = false;

            logic.DeleteSessionCookie();

            Task.Run(async () =>
            {
                success = await logic.SetSessionCookie(
                    _testContext.Properties["dpTestLogin"].ToString(),
                    _testContext.Properties["dpTestPassword"].ToString()
                    );
            }).GetAwaiter().GetResult();

            Assert.AreNotEqual(success, false);

            //OLD
            List<Notification> not = null;
            Task.Run(async () =>
            {
                not = await logic.GetNotifications();
            }).GetAwaiter().GetResult();

            //Assert.IsNotNull(not);

            //Task.Run(async () =>
            //{
            //    await logic.ReadNotify(not.FirstOrDefault().Id, cookie);
            //}).GetAwaiter().GetResult();
        }
    }
}
