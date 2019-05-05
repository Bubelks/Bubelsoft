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
        TokenInfo CreateFor(User user);
    }

    internal class BubelSoftJwtToken: IBubelSoftJwtToken
    {
        private readonly IConfigurationRoot _configuration;

        public BubelSoftJwtToken(IConfigurationRoot configuration)
        {
            _configuration = configuration;
        }

        public TokenInfo CreateFor(User user)
        {
            var userRolesString = JsonConvert.SerializeObject(user.Roles);
            var claims = new[]
            {
                new Claim(JwtCustomClaimNames.UserId, user.Id.ToString()),
                new Claim(JwtCustomClaimNames.FirstName, user.FirstName),
                new Claim(JwtCustomClaimNames.LastName, user.LastName),
                new Claim(JwtCustomClaimNames.Email, user.Email),
                new Claim(JwtCustomClaimNames.CompanyId, user.CompanyId.Value.ToString()),
                new Claim(JwtCustomClaimNames.CompanyRole, ((int)user.CompanyRole).ToString()),
                new Claim(JwtCustomClaimNames.UserRoles, userRolesString),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddMinutes(30);
            var token = new JwtSecurityToken(
                issuer: _configuration["Tokens:Issuer"],
                audience: _configuration["Tokens:Audience"],
                claims: claims,
                expires: expiration,
                signingCredentials: credentials
            );

            return new TokenInfo
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };
        }
    }

    public struct TokenInfo
    {
        public string Token;
        public DateTime Expiration;
    }
}
