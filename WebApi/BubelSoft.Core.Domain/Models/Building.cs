using System;
using System.Collections.Generic;
using System.Linq;

namespace BubelSoft.Core.Domain.Models
{
    public class Building
    {

        public BuildingId Id { get; private set; }

        public string Name { get; }

        public Company MainContractor { get; }

        public IEnumerable<Company> SubContractors { get; }

        public Building(string name, Company mainContractor, IEnumerable<Company> subContractors = null)
        {
            Name = name;
            MainContractor = mainContractor ?? throw new ArgumentNullException(nameof(mainContractor));
            SubContractors = subContractors ?? new List<Company>();
        }

        public Building(BuildingId id, string name, Company mainContractor, IEnumerable<Company> subContractors = null): this(name, mainContractor, subContractors)
        {
            Id = id;
        }

        public void SetId(BuildingId id)
        {
            if (Id.Value != 0)
                throw new InvalidOperationException("Id is already set");
            Id = id;
        }

        public bool IsOwnedBy(User user)
        {
            return MainContractor.Id == user.CompanyId;
        }

        public bool CanAccess(User user)
        {
            return user.Roles.Any(r => r.BuildingId == Id) &&
                (SubContractors.Any(c => c.Id == user.CompanyId) || MainContractor.Id == user.CompanyId);
        }
    }
}