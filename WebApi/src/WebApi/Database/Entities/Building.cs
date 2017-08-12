using System.ComponentModel.DataAnnotations;

namespace WebApi.Database.Entities
{
    public class Building
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public Company MainContractor { get; set; }
    }
}