using BubelSoft.Core.Domain.Models;
using BubelSoft.Core.Infrastructure.Controllers.Security;
using BubelSoft.Core.Infrastructure.Database.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;

namespace BubelSoft.Core.Infrastructure.UnitTests
{
    [TestFixture]
    public class UserControllerTests
    {
        private UserController _userController;
        private IUserRepository _userRepository;
        private IConfigurationRoot _configuration;

        [SetUp]
        public void SetUp()
        {
            _userRepository = Substitute.For<IUserRepository>();
            _configuration = Substitute.For<IConfigurationRoot>();

            _userController = new UserController(_userRepository, _configuration);
        }

        [Test]
        public void Login_BadRequest_UserNotExists()
        {
            const string userName = "userName";
            _userRepository.GetForLogIn(userName).ReturnsNull();

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
            const string userName = "userName";
            const string password = "password";
            var logInInfo = new UserLogInInfo
            {
                UserName = userName,
                Password = password,
                Id = 12
            };

            var passwordHash = new PasswordHasher<UserLogInInfo>().HashPassword(logInInfo, password);
            _userRepository.GetForLogIn(userName).Returns(new UserLogInInfo
            {
                UserName = userName,
                Password = passwordHash
            });
            
            var response = _userController.LogIn(new UserLogInInfo
            {
                UserName = userName,
                Password = "wrong"
            });

            Assert.That(response, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        public void Login_Ok_UserNameAndPasswordAreCorrect()
        {
            const string userName = "userName";
            const string password = "password";
            var logInInfo = new UserLogInInfo
            {
                UserName = userName,
                Password = password,
                Id = 12
            };

            var passwordHash = new PasswordHasher<UserLogInInfo>().HashPassword(logInInfo, password);
            _userRepository.GetForLogIn(userName).Returns(new UserLogInInfo
            {
                UserName = userName,
                Password = passwordHash
            });

            _configuration[Arg.Any<string>()].Returns("configconfigconfigconfigconfig");

            var response = _userController.LogIn(logInInfo);

            Assert.That(response, Is.TypeOf<OkObjectResult>());
        }

        [Test]
        public void Register_NotFound_UserNotExists()
        {
            var userId = new UserId(13);
            _userRepository.Get(userId).ReturnsNull();

            var response = _userController.Register(new UserRegisterInfo());

            Assert.That(response, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public void Register_Ok_UserExists()
        {
            var userId = new UserId(13);
            _userRepository.Get(userId).Returns(new User(userId, "", "", "", UserCompanyRole.Admin, "", ""));

            var response = _userController.Register(new UserRegisterInfo
            {
                Id = userId.Value,
                CompanyRole = UserCompanyRole.Admin,
                Email = "",
                FirstName = "",
                LastName = "",
                Password = "pass",
                PhoneNumber = "",
                Username = "name"
            });

            Assert.That(response, Is.TypeOf<OkResult>());
        }

        [Test]
        public void Get_NotFound_UserNotExists()
        {
            var userId = new UserId(13);
            _userRepository.Get(userId).ReturnsNull();

            var response = _userController.Get(userId.Value);

            Assert.That(response, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public void Get_Ok_UserExists()
        {
            var userId = new UserId(13);
            _userRepository.Get(userId).Returns(new User(userId, "", "", "", UserCompanyRole.Admin, "", ""));

            var response = _userController.Get(userId.Value);

            Assert.That(response, Is.TypeOf<OkObjectResult>());
        }
    }
}
