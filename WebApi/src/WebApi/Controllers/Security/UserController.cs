using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WebApi.Controllers.Interfaces.Security;
using WebApi.Database.Repositories.Interfaces;

namespace WebApi.Controllers.Security
{
    [Route("api/[controller]")]
    public class UserController: Controller, IUserController
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfigurationRoot _configuration;

        public UserController(IUserRepository userRepository, IConfigurationRoot configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        [Route("login")]
        public IActionResult LogIn(UserLogInInfo userInfo)
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
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
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
    }

    public class UserLogInInfo
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}