using System;
using System.Collections.Generic;
using System.Security.Claims;
using BubelSoft.Core.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace BubelSoft.Security
{
    public interface IUserSession
    {
        User User { get; }
    }

    public class UserSession: IUserSession
    {
        public User User { get; private set; }

        public static void UpdateUser(ClaimsPrincipal claimsPrincipal, HttpContext context)
        {
            var user = new User(
                new UserId(Convert.ToInt32(claimsPrincipal.FindFirst(JwtCustomClaimNames.UserId).Value)),
                claimsPrincipal.FindFirst(JwtCustomClaimNames.FirstName).Value,
                claimsPrincipal.FindFirst(JwtCustomClaimNames.LastName).Value,
                (UserCompanyRole) Convert.ToInt32(claimsPrincipal.FindFirst(JwtCustomClaimNames.CompanyRole).Value),
                claimsPrincipal.FindFirst(JwtCustomClaimNames.Email).Value);
            user.From(new CompanyId(Convert.ToInt32(claimsPrincipal.FindFirst(JwtCustomClaimNames.CompanyId).Value)));

            var roles = JsonConvert.DeserializeObject<IList<User.UserRole>>(claimsPrincipal.FindFirst(JwtCustomClaimNames.UserRoles).Value);
            foreach (var role in roles)
                user.AddRole(role.BuildingId, role.UserBuildingRole);

            if (context.RequestServices.GetService<IUserSession>() is UserSession session)
                session.User = user;
        }
    }
}