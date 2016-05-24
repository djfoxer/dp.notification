using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using djfoxer.dp.notification.Core;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using djfoxer.dp.notification.Core.Logic;
using djfoxer.dp.notification.Core.Model;

namespace djfoxer.dp.notification.Test
{
    [TestClass]
    public class UnitTest1
    {
        private static TestContext _testContext;
        private static DpLogic logic = null;

        [ClassInitialize]
        public static void SetupTests(TestContext testContext)
        {
            _testContext = testContext;
            logic = new DpLogic();
        }


        private bool Login()
        {
            Tuple<bool, DateTime?> success = null;

            Task.Run(async () =>
            {
                success = await logic.SetSessionCookie(
                    _testContext.Properties["dpTestLogin"].ToString(),
                    _testContext.Properties["dpTestPassword"].ToString()
                    );
            }).GetAwaiter().GetResult();

            return success.Item1;
        }

        [TestMethod]
        public void GetNotification()
        {



            //Tuple<bool, DateTime?> success = null;


            logic.DeleteSessionCookie();

            bool loginSuccess = Login();

            Assert.AreNotEqual(loginSuccess, false);

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

        [TestMethod]
        public void GetBlogStatics()
        {

            bool loginSuccess = Login();

            Task.Run(async () =>
            {
                var x = await logic.GetFullBlogStatistic();
            }).GetAwaiter().GetResult();



        }
    }
}
