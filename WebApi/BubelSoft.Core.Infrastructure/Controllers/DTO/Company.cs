using System.Collections.Generic;
using BubelSoft.Core.Domain.Models;

namespace BubelSoft.Core.Infrastructure.Controllers.DTO
{
    public class Company: CompanyInfo
    {
        public IEnumerable<User> Workers { get; set; }
        public bool CanManageWorkers { get; set; }
        public bool CanEdit { get; set; }
    }

    public class BuildingCompany: CompanyInfo
    {
        public int BuildingId { get; set; }
    }

    public class CompanyInfo: CompanyBase
    {
        public string Nip { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
        public string Street { get; set; }
        public string PlaceNumber { get; set; }
    }

    public class CompanyBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public UserCompanyRole CompanyRole { get; set; }
        public int Id { get; set; }
    }
}