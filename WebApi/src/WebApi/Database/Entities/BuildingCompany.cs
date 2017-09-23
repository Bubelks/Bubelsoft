using System.Collections.Generic;

namespace WebApi.Database.Entities
{
    public class BuildingCompany
    {
        public int BuildingId { get; set; }
        public Building Building { get; set; }

        public int CompanyId { get; set; }
        public Company Company { get; set; }

        public List<UserRole> Users { get; set; }

        public ContractType ContractType { get; set; }
    }

    public enum ContractType
    {
        MainContractor,
        SubContractor
    }
}
