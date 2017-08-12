using System.ComponentModel.DataAnnotations;
using WebApi.Domain;

namespace WebApi.Database.Entities
{
    public class Role
    {
        public int Id { get; set; }
        [Required]
        public RoleType Type { get; set; }
    }
}