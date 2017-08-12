using System.ComponentModel.DataAnnotations;

namespace WebApi.Database.Entities
{
    public class Company
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}