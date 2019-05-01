using BubelSoft.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Controllers.DTO;

namespace WebApi.Controllers.Security
{
    [Route("api/[controller]")]
    public class UserController: ControllerBase
    {
        private readonly IUserLoginRepository _userRepository;
        private readonly IBubelSoftJwtToken _bubelSoftJwtToken;
        private readonly IBubelSoftUserPassword _bubelSoftUserPassword;
        private readonly IUserSession _userSession;

        public UserController(
            IUserLoginRepository userRepository,
            IBubelSoftJwtToken bubelSoftJwtToken,
            IBubelSoftUserPassword bubelSoftUserPassword,
            IUserSession userSession)
        {
            _userRepository = userRepository;
            _bubelSoftUserPassword = bubelSoftUserPassword;
            _bubelSoftJwtToken = bubelSoftJwtToken;
            _userSession = userSession;
        }

        [Route("login")]
        [HttpPost]
        public IActionResult LogIn([FromBody]UserLogInInfo userInfo)
        {
            var (user, password) = _userRepository.GetForLogIn(userInfo.Email);

            if (user == null)
                return BadRequest("User not found");

            if(!_bubelSoftUserPassword.Verify(userInfo))
                return BadRequest("Password is invalid");
            
            return Ok(_bubelSoftJwtToken.CreateFor(user));
        }

        [Authorize]
        [Route("current")]
        [HttpGet]
        public IActionResult GetCurrent()
        {
            var currentUser = _userSession.User;
            var dto = new User
            {
                FirstName = currentUser.FirstName,
                LastName = currentUser.LastName,
                Email = currentUser.Email
            };
            return Ok(dto);
        }

        [Route("register")]
        public IActionResult Register([FromBody] UserLogInInfo userInfo)
        {
            //var newId = 0;
            //var userId = new UserId(0);
            //var user = _userRepository.Get(userId);
            //if (user == null)
            //    return BadRequest();

            //var passwordHasher = new PasswordHasher<UserLogInInfo>();
            //var userLogInInfo = new UserLogInInfo
            //{
            //    Email = userInfo.Email,
            //    Password = userInfo.Password,
            //    Id = newId
            //};
            //var passwordHash = passwordHasher.HashPassword(userLogInInfo, userLogInInfo.Password);

            //var userDomain = new User(
            //    userInfo.Email,
            //    "userInfo.FirstName",
            //    "userInfo.LastName",
            //    UserCompanyRole.Admin,
            //    "userInfo.Email",
            //    "userInfo.PhoneNumber");

            //userDomain.From(user.CompanyId);
            //userDomain.SetId(userId);

            //foreach (var userRole in user.UserRoles)
            //{
            //    userDomain.AddRole(userRole.BuildingId, userRole.UserBuildingRole);
            //}
            //_userRepository.Save(userDomain, passwordHash);

            return Ok();
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            //var userId = new UserId(id);
            //var user = _userRepository.Get(userId);
            //if (user == null)
            //    return BadRequest();

            return Ok();
        }
    }
}