using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
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
                    Id = entity.Id,
                    UserName = entity.Name,
                    Password = entity.Password
                };
        }

        public IEnumerable<User> GetWorkers(CompanyId companyId)
        {
            return _context.Users.Where(u => u.CompanyId == companyId.Value).Select(e => Create(e));
        }

        public UserId Save(User user, string password = null)
        {
            var entity = Get(user.Id.Value);
            if (entity == null)
            {
                entity = new Entites.User{
                    Name = user.Name,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    CompanyId = user.CompanyId.Value,
                    CompanyRole = user.CompanyRole,
                    PhoneNumber = user.PhoneNumber,
                    Email = user.Email,
                    Password = password,
                    Roles = user.Roles.Select(r => new Entites.UserRole
                    {
                        BuildingId = r.BuildingId.Value,
                        CompanyId = user.CompanyId.Value,
                        UserBuildingRole = r.UserBuildingRole

                    }).ToList()
                };
                _context.Users.Add(entity);
            }
            else
                Update(entity, user);

            _context.SaveChanges();

            if (user.Id.Value == 0)
                user.SetId(new UserId(entity.Id));

            return new UserId(entity.Id);
        }

        private static User Create(Entites.User entity)
        {
            var user = new User(new UserId(entity.Id), entity.Name, entity.FirstName, entity.LastName, entity.CompanyRole, entity.Email, entity.PhoneNumber);

            if(entity.CompanyId != null)
                user.From(new CompanyId(entity.CompanyId.Value));

            entity.Roles.ForEach(r => user.AddRole(new BuildingId(r.BuildingId), r.UserBuildingRole));

            return user;
        }

        private static void Update(Entites.User entity, User user)
        {
            entity.Name = user.Name;
            entity.FirstName = user.FirstName;
            entity.LastName = user.LastName;
            entity.CompanyId = user.CompanyId.Value;
            entity.PhoneNumber = user.PhoneNumber;
            entity.Email = user.Email;
            entity.CompanyRole = user.CompanyRole;
            entity.Roles = user.Roles.Select(r => new Entites.UserRole
            {
                BuildingId = r.BuildingId.Value,
                CompanyId = user.CompanyId.Value,
                UserBuildingRole = r.UserBuildingRole,
                UserId = entity.Id

            }).ToList();
        }

        private Entites.User Get(int id)
        {
            return _context.Users
                .Include(u => u.Roles)
                .FirstOrDefault(u => u.Id == id);
        }
    }
}