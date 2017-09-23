using System.Linq;
using WebApi.Controllers.Security;
using WebApi.Database.Repositories.Interfaces;
using WebApi.Domain.Models;
using Entites = WebApi.Database.Entities;

namespace WebApi.Database.Repositories
{
    internal class UserRepository : IUserRepository
    {
        private readonly MainContext _context;

        public UserRepository(MainContext context)
        {
            _context = context;
        }

        public User Get(UserId id)
        {
            var entity = Get(id.Value);
            return entity == null ? null : Create(entity);
        }

        public UserLogInInfo GetForLogIn(string userName)
        {
            var entity = _context.Users.FirstOrDefault(u => u.Name == userName);
            return entity == null
                ? null
                : new UserLogInInfo
                {
                    UserName = entity.Name,
                    Password = entity.Password
                };
        }

        public UserId Save(User user, string password = "")
        {
            var entity = Get(user.Id.Value);
            if (entity == null)
            {
                entity = new Entites.User{
                    Name = user.Name,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    CompanyId = user.CompanyId.Value,
                    Password = password};
                _context.Users.Add(entity);
            }
            else
                Update(entity, user);

            _context.SaveChanges();
            return new UserId(entity.Id);
        }

        private static User Create(Entites.User entity)
        {
            var user = new User(new UserId(entity.Id), entity.Name, entity.FirstName, entity.LastName, new CompanyId(entity.CompanyId));

            return user;
        }

        private static void Update(Entites.User entity, User user)
        {
            entity.Name = user.Name;
            entity.FirstName = user.FirstName;
            entity.LastName = user.LastName;
            entity.CompanyId = user.CompanyId.Value;
            entity.Roles.Clear();
        }

        private Entites.User Get(int id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == id);
        }
    }
}