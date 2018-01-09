using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApi.Domain.Models;

namespace WebApi.Database.Entities
{
    public class User: Entity
    {
        public User()
        {
            Roles = new List<UserRole>();
        }

        public string Name { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public Company Company { get; set; }
        public int? CompanyId { get; set; }
        public UserCompanyRole CompanyRole { get; set; }

        public List<UserRole> Roles { get; set; }
    }
}