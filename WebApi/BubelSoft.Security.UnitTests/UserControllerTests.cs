using BubelSoft.Core.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using WebApi.Controllers;
using User = BubelSoft.Core.Domain.Models.User;

namespace BubelSoft.Security.UnitTests
{
    [TestFixture]
    public class UserControllerTests
    {
        private UserController _userController;
        private IUserLoginRepository _userRepository;
        private IBubelSoftJwtToken _bubelSoftJwtToken;
        private IUserSession _userSession;
        private IBubelSoftUserPassword _bubelSoftPassword;

        [OneTimeSetUp]
        public void SetUp()
        {
            _userRepository = Substitute.For<IUserLoginRepository>();
            _bubelSoftJwtToken = Substitute.For<IBubelSoftJwtToken>();
            _bubelSoftPassword = Substitute.For<IBubelSoftUserPassword>();
            _userSession = Substitute.For<IUserSession>();

            _userController = new UserController(
                _userRepository,
                _bubelSoftJwtToken,
                _bubelSoftPassword,
                _userSession);
        }

        [Test]
        public void Login_BadRequest_UserNotExists()
        {
            const string userName = "userName";
            _userRepository.GetForLogIn(userName).Returns((null, null));

            var response = _userController.LogIn(new UserLogInInfo
            {
                UserName = "userName",
                Password = "pass"
            });

            Assert.That(response, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        public void Login_BadRequest_PasswordIsWrong()
        {
            var user = CreateUser(12, "BAD_User");
            var userLogInInfo = StubLogInRepository(user, goodPassword: false);

            var response = _userController.LogIn(userLogInInfo);

            Assert.That(response, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        public void Login_Ok_UserNameAndPasswordAreCorrect()
        {
            var user = CreateUser(12, "OK_User");
            var userLogInInfo = StubLogInRepository(user, goodPassword: true);
            _bubelSoftJwtToken.CreateFor(user).Returns("configconfigconfigconfigconfig");

            var response = _userController.LogIn(userLogInInfo);

            Assert.That(response, Is.TypeOf<OkObjectResult>());
        }

        [Test]
        public void GetCurrent_OkWithUser_UserIsLoggedIn()
        {
            var userName = "userName";
            _userSession.User.Returns(CreateUser(34, userName));
            
            var response = _userController.GetCurrent();

            Assert.That(response, Is.TypeOf<OkObjectResult>());
            var result = (response as ObjectResult).Value as WebApi.Controllers.DTO.User;
            Assert.That(result.Name, Is.EqualTo(userName));
            Assert.That(result.FirstName, Is.EqualTo("firstName"));
            Assert.That(result.LastName, Is.EqualTo("lastName"));
            Assert.That(result.Email, Is.EqualTo("email"));
        }

        private UserLogInInfo StubLogInRepository(User user, bool goodPassword)
        {
            const string passwordHash = "passwordHash";
            _userRepository.GetForLogIn(user.Name).Returns((user, passwordHash));

            var userLogInInfo = new UserLogInInfo
            {
                UserName = user.Name,
                Password = "password"
            };

            _bubelSoftPassword.Verify(userLogInInfo, passwordHash).Returns(goodPassword);

            return userLogInInfo;
        }

        private static User CreateUser(int userId, string userName) =>
            new User(
                new UserId(userId),
                userName,
                "firstName",
                "lastName",
                UserCompanyRole.Admin,
                "email",
                "890876789");
    }
}
