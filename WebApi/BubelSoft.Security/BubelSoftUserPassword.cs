using Microsoft.AspNetCore.Identity;

namespace BubelSoft.Security
{
    public interface IBubelSoftUserPassword
    {
        bool Verify(UserLogInInfo userLogInInfo, string passwordHash);
        string Hash(UserLogInInfo userLogInInfo);
    }

    internal class BubelSoftUserPassword: IBubelSoftUserPassword
    {
        private readonly PasswordHasher<UserLogInInfo> _passwordHasher;

        public BubelSoftUserPassword()
        {
            _passwordHasher = new PasswordHasher<UserLogInInfo>();
        }

        public bool Verify(UserLogInInfo userLogInInfo, string passwordHash) =>
            _passwordHasher
                .VerifyHashedPassword(userLogInInfo, passwordHash, userLogInInfo.Password) != PasswordVerificationResult.Failed;

        public string Hash(UserLogInInfo userLogInInfo) =>
            _passwordHasher.HashPassword(userLogInInfo, userLogInInfo.Password);
    }
    
    public class UserLogInInfo
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
