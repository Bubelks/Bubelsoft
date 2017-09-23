namespace WebApi.Domain.Models
{
    public class User
    {
        public UserId Id { get; }
        public string Name { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public CompanyId CompanyId { get; }

        public User(UserId id, string name, string firstName, string lastName, CompanyId companyId) : this(name, firstName, lastName, companyId)
        {
            Id = id;
        }

        public User(string name, string firstName, string lastName, CompanyId companyId)
        {
            Name = name;
            FirstName = firstName;
            LastName = lastName;
            CompanyId = companyId;
        }
    }

    public struct UserId
    {
        public UserId(int value)
        {
            Value = value;
        }

        public int Value { get;  }
    }
}