using System.Collections.Generic;
using BubelSoft.Core.Domain.Models;

namespace BubelSoft.Core.Infrastructure.Database.Repositories.Interfaces
{
    public interface IUserRepository
    {
        User Get(UserId id);
        UserId Save(User user, string password = "");
        IEnumerable<User> GetWorkers(CompanyId companyId);
        IEnumerable<User> GetBuildingWorkers(BuildingId buildingId, CompanyId companyId);
    }
}