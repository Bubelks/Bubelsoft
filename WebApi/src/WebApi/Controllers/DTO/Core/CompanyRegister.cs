namespace WebApi.Controllers.DTO.Core
{
    public class CompanyRegister
    {
        public NewCompany Company { get; set; }
        public NewAdministrator Administrator { get; set; }
    }

    public class NewCompany
    {
        public string Name { get; set; }
        public string Number { get; set; }
    }

    public class NewAdministrator
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}