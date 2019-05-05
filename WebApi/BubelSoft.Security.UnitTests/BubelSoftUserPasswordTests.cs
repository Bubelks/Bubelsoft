using NUnit.Framework;

namespace BubelSoft.Security.UnitTests
{
    [TestFixture]
    public class BubelSoftUserPasswordTests
    {
        [Test]
        public void Verify_ReturnTrue_WhenPasswordsAreMatched()
        {
            var userLogin = new UserLogInInfo
            {
                Email = "user@mail.com",
                Password = "good"
            };
            var bubelSoftUserPassword = new BubelSoftUserPassword();

            var result = bubelSoftUserPassword.Verify(userLogin, bubelSoftUserPassword.Hash(userLogin));

            Assert.That(result, Is.True, "Password should match");
        }
        [Test]
        public void Verify_ReturnTrue_WhenPasswordsAreNotMatched()
        {
            var userLogin = new UserLogInInfo
            {
                Email = "user@mail.com",
                Password = "good"
            };
            var bubelSoftUserPassword = new BubelSoftUserPassword();
            var passwordHash = bubelSoftUserPassword.Hash(userLogin);
            userLogin.Password = "wrong";

            var result = bubelSoftUserPassword.Verify(userLogin, passwordHash);

            Assert.That(result, Is.False, "Password should not match");
        }
    }
}
