using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Database.Entities
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        [Required]
        public Company Company { get; set; }
        public int CompanyId { get; set; }
        public List<UserRole> Roles { get; set; }
    }
}