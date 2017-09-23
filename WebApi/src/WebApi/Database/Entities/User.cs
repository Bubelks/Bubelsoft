using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Database.Entities
{
    public class User: Entity
    {
        public User()
        {
            Roles = new List<UserRole>();
        }

        [Required]
        public string Name { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Password { get; set; }

        public Company Company { get; set; }
        public int CompanyId { get; set; }

        public List<UserRole> Roles { get; set; }
    }
}