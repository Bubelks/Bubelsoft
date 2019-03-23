using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BubelSoft.Core.Infrastructure.Database.Entities
{
    public class Company: Entity
    {
        [Required]
        public string Name { get; set; }
        public List<BuildingCompany> Buildings { get; set; }
        public string Nip { get; set; }
        public string PhoneNumber { get; set; }
        public string EMail { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
        public string Street { get; set; }
        public string PlaceNumber { get; set; }
    }
}