using System.Collections.Generic;
using WebApi.Controllers.BuildingContext;
using WebApi.Controllers.Security;
using WebApi.Domain.Models;

namespace WebApi.Database.Repositories.Interfaces
{
    public interface IUserRepository
    {
        User Get(UserId id);
        UserId Save(User user, string password = "");
        UserLogInInfo GetForLogIn(string userName);
        IEnumerable<User> GetWorkers(CompanyId companyId);
        IEnumerable<User> GetBuildingWorkers(BuildingId buildingId, CompanyId companyId);
    }
}