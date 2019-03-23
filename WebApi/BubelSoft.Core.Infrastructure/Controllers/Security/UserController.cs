using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BubelSoft.Core.Domain.Models;
using BubelSoft.Core.Infrastructure.Database.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using User = BubelSoft.Core.Infrastructure.Controllers.DTO.User;

namespace BubelSoft.Core.Infrastructure.Controllers.Security
{
    [Route("api/[controller]")]
    public class UserController: Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfigurationRoot _configuration;

        public UserController(IUserRepository userRepository, IConfigurationRoot configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        [Route("login")]
        public IActionResult LogIn([FromBody]UserLogInInfo userInfo)
        {
            var user = _userRepository.GetForLogIn(userInfo.UserName);

            if (user == null)
                return BadRequest("User not found");
            
            var passwordHasher = new PasswordHasher<UserLogInInfo>();
            if (passwordHasher.VerifyHashedPassword(user, user.Password, userInfo.Password) ==
                PasswordVerificationResult.Failed)
                return BadRequest("Password is invalid");

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Tokens:Issuer"],
                audience: _configuration["Tokens:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds
            );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });
        }

        [Route("register")]
        public IActionResult Register([FromBody] UserRegisterInfo userInfo)
        {
            var userId = new UserId(userInfo.Id);
            var user = _userRepository.Get(userId);
            if (user == null)
                return NotFound();

            var passwordHasher = new PasswordHasher<UserLogInInfo>();
            var userLogInInfo = new UserLogInInfo
            {
                UserName = userInfo.Username,
                Password = userInfo.Password,
                Id = userInfo.Id
            };
            var passwordHash = passwordHasher.HashPassword(userLogInInfo, userLogInInfo.Password);

            var userDomain = new Domain.Models.User(
                userInfo.Username,
                userInfo.FirstName,
                userInfo.LastName,
                userInfo.CompanyRole,
                userInfo.Email,
                userInfo.PhoneNumber);

            userDomain.From(user.CompanyId);
            userDomain.SetId(userId);

            foreach (var userRole in user.Roles)
            {
                userDomain.AddRole(userRole.BuildingId, userRole.UserBuildingRole);
            }
            _userRepository.Save(userDomain, passwordHash);

            return Ok();
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var userId = new UserId(id);
            var user = _userRepository.Get(userId);

            if (user == null)
                return NotFound();

            return Ok(new User
            {
                Username = user.Name,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                CompanyRole = user.CompanyRole
            });
        }
    }

    public class UserRegisterInfo: DTO.User
    {
        public string Password { get; set; }
    }
}