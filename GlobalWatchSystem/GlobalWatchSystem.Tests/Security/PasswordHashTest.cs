using GlobalWatchSystem.Security;
using NUnit.Framework;

namespace GlobalWatchSystem.Tests.Security
{
    [TestFixture]
    public class PasswordHashTest
    {
        [Test]
        public void TestHashPassword()
        {
            var hashedPasswordForddou = "1000:ZhCLFlDuxow0oXPwkF5rtVlmikdfDZGm:r0qUpQ5oLKZ9PJQRwZJRs+x2hjWGk4ZY";
            Assert.AreEqual(true,PasswordHash.ValidatePassword("ddou",hashedPasswordForddou));
        }

    }
}