using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Database.Entities
{
    public class Company: Entity
    {
        [Required]
        public string Name { get; set; }

        public List<BuildingCompany> Buildings { get; set; }
    }
}