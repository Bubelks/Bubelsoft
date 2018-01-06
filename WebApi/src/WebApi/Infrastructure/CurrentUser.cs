using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using WebApi.Database.Repositories.Interfaces;
using WebApi.Domain.Models;

namespace WebApi.Infrastructure
{
    public interface ICurrentUser
    {
        UserId Id { get; }
        User User { get; }
    }

    public class CurrentUser: ICurrentUser
    {
        private readonly IUserRepository _userRepository;

        private User _user;
        public User User
        {
            get
            {
                if (_user == null)
                    _user = _userRepository.Get(Id);

                if (_user == null)
                    throw new ArgumentNullException($"There is no user with id: {Id}");

                return _user;
            }
        }

        public UserId Id { get; }

        public CurrentUser(IHttpContextAccessor context, IUserRepository userRepository)
        {
            _userRepository = userRepository;
            Id = new UserId(int.Parse(context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value));
        }
    }
}
