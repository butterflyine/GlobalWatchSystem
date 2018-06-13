using GlobalWatchSystem.Helpers;
using GlobalWatchSystem.Models;
using NUnit.Framework;

namespace GlobalWatchSystem.Tests.Helpers
{
    [TestFixture]
    public class ChangeInfoTest
    {
        [Test]
        public void TestChangeInfo()
        {
            var user = new User {UserName = "asdf", Email = "email address"};
            string changeInfo = new ChangeInfo()
                .AddChange(() => user.UserName)
                .AddChange(() => user.Email)
                .AddChange(() => user.AreaId)
                .ToJson();

            Assert.AreEqual(@"{""UserName"":""asdf"",""Email"":""email address"",""AreaId"":0}", changeInfo);
        }
    }
}