using NUnit.Framework;
using WebApi.Infrastructure.Email;

namespace WebApi.UnitTests.Infrastructure.Email
{
    [TestFixture]
    public class EmailSenderTests
    {
        [Test]
        public void T()
        {
            var sender = new EmailSender();
            sender.Send();
        }
    }
}
