using System.Collections.Generic;
using System.Linq;
using BubelSoft.Core.Domain.Models;
using BubelSoft.Core.Infrastructure.Database.Entities;
using BubelSoft.Core.Infrastructure.Database.Repositories.Interfaces;
using BubelSoft.Security;
using Microsoft.EntityFrameworkCore;
using User = BubelSoft.Core.Domain.Models.User;

namespace BubelSoft.Core.Infrastructure.Database.Repositories
{
    public class UserRepository : IUserRepository, IUserLoginRepository
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

        public IEnumerable<User> GetWorkers(CompanyId companyId)
        {
            return _context.Users.Where(u => u.CompanyId == companyId.Value).Select(e => Create(e));
        }

        public IEnumerable<User> GetBuildingWorkers(BuildingId buildingId, CompanyId companyId)
        {
            return _context.Users
                .Where(u => u.CompanyId == companyId.Value)
                .Include(u => u.Roles)
                .Where(u => u.Roles.Any(r => r.BuildingId == buildingId.Value))
                .AsEnumerable()
                .Select(Create);
        }

        public UserId Save(User user, string password = null)
        {
            var entity = user.IsNew
                ? new Entities.User()
                : Get(user.Id.Value);

            Update(entity, user, password);

            if (entity.Id == 0)
                _context.Add(entity);
            _context.SaveChanges();

            if (user.Id.Value == 0)
                user.SetId(new UserId(entity.Id));

            return new UserId(entity.Id);
        }
        
        public (User user, string password) GetForLogIn(string userEmail)
        {
            var entity = _context.Users.FirstOrDefault(u => u.Email == userEmail);
            return entity == null
                ? (null, null)
                : (Create(entity), entity.Password);
        }
        
        private static User Create(Entities.User entity)
        {
            var user = new User(new UserId(entity.Id), entity.FirstName, entity.LastName, entity.CompanyRole, entity.Email);

            if(entity.CompanyId != null)
                user.From(new CompanyId(entity.CompanyId.Value));

            entity.Roles.ForEach(r => user.AddRole(new BuildingId(r.BuildingId), r.UserBuildingRole));

            return user;
        }

        private static void Update(Entities.User entity, User user, string passwordHash = null)
        {
            entity.FirstName = user.FirstName;
            entity.LastName = user.LastName;
            entity.CompanyId = user.CompanyId.Value;
            entity.Email = user.Email;
            entity.CompanyRole = user.CompanyRole;

            entity.Roles.RemoveAll(ur => user.Roles.All(r =>
                r.BuildingId.Value != ur.BuildingId && r.UserBuildingRole != ur.UserBuildingRole &&
                user.CompanyId.Value != ur.CompanyId));

            entity.Roles.AddRange(user.Roles
                .Where(r => 
                    entity.Roles.All(ur => r.BuildingId.Value != ur.BuildingId && r.UserBuildingRole != ur.UserBuildingRole && user.CompanyId.Value != ur.CompanyId))
                .Select(r => new UserRole
                {
                    BuildingId = r.BuildingId.Value,
                    CompanyId = user.CompanyId.Value,
                    UserBuildingRole = r.UserBuildingRole,
                    UserId = entity.Id

                }));

            if (passwordHash != null)
                entity.Password = passwordHash;
        }

        private Entities.User Get(int id)
        {
            return _context.Users
                .Include(u => u.Roles)
                .FirstOrDefault(u => u.Id == id);
        }
    }
}