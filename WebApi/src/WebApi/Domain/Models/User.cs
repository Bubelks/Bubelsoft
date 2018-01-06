namespace WebApi.Domain.Models
{
    public class User
    {
        public UserId Id { get; }
        public string Name { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string Email { get; }
        public string PhoneNumber { get; }
        public CompanyId CompanyId { get; private set; }
        public UserCompanyRole CompanyRole { get; }

        public User(UserId id, string name, string firstName, string lastName, UserCompanyRole companyRole, string email, string phoneNumber) : this(name, firstName, lastName, companyRole, email, phoneNumber)
        {
            Id = id;
        }

        public User(string name, string firstName, string lastName, UserCompanyRole companyRole, string email, string phoneNumber)
        {
            Name = name;
            FirstName = firstName;
            LastName = lastName;
            CompanyRole = companyRole;
            Email = email;
            PhoneNumber = phoneNumber;
        }

        public void From(CompanyId companyId)
        {
            CompanyId = companyId;
        }

        public bool CanEdit(CompanyId companyId) => companyId == CompanyId &&
                                                    CompanyRole == UserCompanyRole.Admin;

        public bool CanManageWorkers(CompanyId companyId) => companyId == CompanyId &&
                                                             (CompanyRole == UserCompanyRole.Admin || CompanyRole == UserCompanyRole.UserAdmin);
    }

    public struct UserId
    {
        public UserId(int value)
        {
            Value = value;
        }

        public int Value { get;  }
    }

    public enum UserCompanyRole
    {
        Admin,
        UserAdmin,
        Worker
    }
}