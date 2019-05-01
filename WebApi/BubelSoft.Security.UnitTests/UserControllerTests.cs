using BubelSoft.Core.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using WebApi.Controllers.Security;
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

            var response = _userController.LogIn(new UserLogInInfo{
                Email = "userName",
                Password = "pass"
                });

            Assert.That(response, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        public void Login_BadRequest_PasswordIsWrong()
        {
            var user = CreateUser(12);
            var userLogInInfo = StubLogInRepository(user, goodPassword: false);

            var response = _userController.LogIn(userLogInInfo);

            Assert.That(response, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        public void Login_Ok_UserNameAndPasswordAreCorrect()
        {
            var user = CreateUser(12);
            var userLogInInfo = StubLogInRepository(user, goodPassword: true);
            _bubelSoftJwtToken.CreateFor(user).Returns("configconfigconfigconfigconfig");

            var response = _userController.LogIn(userLogInInfo);

            Assert.That(response, Is.TypeOf<OkObjectResult>());
        }

        [Test]
        public void GetCurrent_OkWithUser_UserIsLoggedIn()
        {
            _userSession.User.Returns(CreateUser(34));
            
            var response = _userController.GetCurrent();

            Assert.That(response, Is.TypeOf<OkObjectResult>());
            var result = (response as ObjectResult).Value as WebApi.Controllers.DTO.User;
            Assert.That(result.FirstName, Is.EqualTo("firstName"));
            Assert.That(result.LastName, Is.EqualTo("lastName"));
            Assert.That(result.Email, Is.EqualTo("email"));
        }

        private UserLogInInfo StubLogInRepository(User user, bool goodPassword)
        {
            const string passwordHash = "passwordHash";
            _userRepository.GetForLogIn(user.Email).Returns((user, passwordHash));

            var userLogInInfo = new UserLogInInfo
            {
                Email = user.Email,
                Password = "password"
            };

            _bubelSoftPassword.Verify(userLogInInfo).Returns(goodPassword);

            return userLogInInfo;
        }

        private static User CreateUser(int userId) =>
            new User(
                new UserId(userId),
                "firstName",
                "lastName",
                UserCompanyRole.Admin,
                "email");
    }
}
