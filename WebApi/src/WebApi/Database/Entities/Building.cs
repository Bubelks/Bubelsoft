using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Database.Entities
{
    public class Building: Entity
    {
        public Building()
        {
            Companies = new List<BuildingCompany>();    
        }

        [Required]
        public string Name { get; set; }

        public List<BuildingCompany> Companies { get; set; }

        public string ConnectionString { get; set; }
        
        public bool? IsReady { get; set; }
    }
}