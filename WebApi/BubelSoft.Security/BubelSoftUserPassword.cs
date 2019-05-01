using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace BubelSoft.Security
{
    public interface IBubelSoftUserPassword
    {
        bool Verify(UserLogInInfo userLogInInfo);
        string Hash(UserLogInInfo userLogInInfo);
    }

    public class BubelSoftUserPassword: IBubelSoftUserPassword
    {
        private readonly PasswordHasher<UserLogInInfo> _passwordHasher;

        public BubelSoftUserPassword()
        {
            _passwordHasher = new PasswordHasher<UserLogInInfo>();
        }

        public bool Verify(UserLogInInfo userLogInInfo)
        {
            var passwordHash = Hash(userLogInInfo);
            return _passwordHasher
                .VerifyHashedPassword(userLogInInfo, passwordHash, userLogInInfo.Password) != PasswordVerificationResult.Failed;
        }

        public string Hash(UserLogInInfo userLogInInfo) =>
            _passwordHasher.HashPassword(userLogInInfo, userLogInInfo.Password);
    }
    
    public class UserLogInInfo
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
