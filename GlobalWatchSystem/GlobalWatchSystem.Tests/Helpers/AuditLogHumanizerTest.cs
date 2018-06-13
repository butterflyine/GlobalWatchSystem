using System.Globalization;
using System.Threading;
using GlobalWatchSystem.Helpers;
using GlobalWatchSystem.Models;
using NUnit.Framework;

namespace GlobalWatchSystem.Tests.Helpers
{
    [TestFixture]
    public class AuditLogHumanizerTest
    {
        [SetUp]
        public void SetUp()
        {
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");
        }

        [Test]
        public void TestHumanize()
        {
            var log = new AuditLog
            {
                Type = "User",
                Changes = @"{""UserName"":""asdf"",""Email"":""email address""}"
            };

            string actual = AuditLogHumanizer.Humanize(log);

            Assert.AreEqual("Name => 'asdf', Email => 'email address'", actual);
        }

        [Test]
        public void TestHumanizeShouldReturnEmptyStringIfThereIsNoChange()
        {
            var log = new AuditLog
            {
                Type = "User",
                Changes = @"{}"
            };

            string actual = AuditLogHumanizer.Humanize(log);

            Assert.AreEqual("", actual);
        }
    }
}