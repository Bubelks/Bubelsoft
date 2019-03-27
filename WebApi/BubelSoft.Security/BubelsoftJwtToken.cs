using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BubelSoft.Core.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace BubelSoft.Security
{
    public interface IBubelSoftJwtToken
    {
        string CreateFor(User user);
    }

    internal class BubelSoftJwtToken: IBubelSoftJwtToken
    {
        private readonly IConfigurationRoot _configuration;

        public BubelSoftJwtToken(IConfigurationRoot configuration)
        {
            _configuration = configuration;
        }

        public string CreateFor(User user)
        {
            var userRolesString = JsonConvert.SerializeObject(user.Roles);
            var claims = new[]
            {
                new Claim(JwtCustomClaimNames.UserId, user.Id.ToString()),
                new Claim(JwtCustomClaimNames.UserName, user.Name),
                new Claim(JwtCustomClaimNames.FirstName, user.FirstName),
                new Claim(JwtCustomClaimNames.LastName, user.LastName),
                new Claim(JwtCustomClaimNames.Email, user.Email),
                new Claim(JwtCustomClaimNames.PhoneNumber, user.PhoneNumber),
                new Claim(JwtCustomClaimNames.CompanyId, user.CompanyId.Value.ToString()),
                new Claim(JwtCustomClaimNames.CompanyRole, ((int)user.CompanyRole).ToString()),
                new Claim(JwtCustomClaimNames.UserRoles, userRolesString),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Tokens:Issuer"],
                audience: _configuration["Tokens:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
