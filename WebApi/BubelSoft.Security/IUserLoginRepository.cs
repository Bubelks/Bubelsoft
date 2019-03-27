using BubelSoft.Core.Domain.Models;

namespace BubelSoft.Security
{
    public interface IUserLoginRepository
    {
        (User user, string password) GetForLogIn(string userName);
    }
}