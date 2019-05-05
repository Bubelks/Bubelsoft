using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace BubelSoft.Security
{
    public interface IBubelSoftUserPassword
    {
        bool Verify(UserLogInInfo userLogin, string passwordHash);
        string Hash(UserLogInInfo userLogin);
    }

    public class BubelSoftUserPassword: IBubelSoftUserPassword
    {
        private readonly PasswordHasher<UserLogInInfo> _passwordHasher;

        public BubelSoftUserPassword()
        {
            _passwordHasher = new PasswordHasher<UserLogInInfo>();
        }

        public bool Verify(UserLogInInfo userLogin, string passwordHash)
        {
            return _passwordHasher
                .VerifyHashedPassword(userLogin, passwordHash, userLogin.Password) != PasswordVerificationResult.Failed;
        }

        public string Hash(UserLogInInfo userLogin) =>
            _passwordHasher.HashPassword(userLogin, userLogin.Password);
    }
    
    public class UserLogInInfo
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
