using WebApi.Controllers.Security;
using WebApi.Domain.Models;

namespace WebApi.Database.Repositories.Interfaces
{
    public interface IUserRepository
    {
        User Get(UserId id);
        UserId Save(User user, string password = "");
        UserLogInInfo GetForLogIn(string userName);
    }
}