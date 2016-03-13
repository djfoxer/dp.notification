using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using djfoxer.dp.notification.Core;
using System.Threading.Tasks;

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
        public void TestMethod1()
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
        }
    }
}
