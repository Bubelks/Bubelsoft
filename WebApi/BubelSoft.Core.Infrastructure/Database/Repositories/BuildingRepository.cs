using System.Collections.Generic;
using System.Linq;
using BubelSoft.Core.Domain.Models;
using BubelSoft.Core.Infrastructure.Database.Entities;
using BubelSoft.Core.Infrastructure.Database.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Building = BubelSoft.Core.Domain.Models.Building;
using Company = BubelSoft.Core.Domain.Models.Company;

namespace BubelSoft.Core.Infrastructure.Database.Repositories
{
    public class BuildingRepository : IBuildingRepository
    {
        private readonly MainContext _context;

        public BuildingRepository(MainContext context)
        {
            _context = context;
        }

        public IEnumerable<Building> GetFor(UserId userId)
        {
            var enities = _context.Buildings
                .Include(b => b.Companies)
                .ThenInclude(bc => bc.Company)
                .Where(b => b.IsReady.Value)
                .Where(b => b.Companies.Any(bc => bc.Users.Any(ur => ur.UserId == userId.Value)));

            return enities.Select(Convert);
        }

        public Building Get(BuildingId id)
        {
            return Convert(GetById(id));
        }

        public BuildingId Save(Building building)
        {
            var dbEntity = GetById(building.Id) ?? new Entities.Building();
            
            dbEntity.Name = building.Name;

            if(dbEntity.Companies.Count > 1)
                dbEntity.Companies.RemoveAll(c => c.ContractType == ContractType.SubContractor && building.SubContractors.All(sc => sc.Id.Value != c.CompanyId));

            if(dbEntity.Companies.Count == 0)
                dbEntity.Companies.Add(
                    new BuildingCompany
                    {
                        CompanyId = building.MainContractor.Id.Value,
                        ContractType = ContractType.MainContractor
                    }
                );

            dbEntity.Companies.AddRange(building.SubContractors
                .Where(sc => dbEntity.Companies.All(c => c.CompanyId != sc.Id.Value))
                .Select(c => new BuildingCompany
                {
                    CompanyId = c.Id.Value,
                    ContractType = ContractType.SubContractor
                })
            );

            if (dbEntity.Id == 0)
                _context.Buildings.Add(dbEntity);
            _context.SaveChanges();

            if (building.Id.Value == 0)
                building.SetId(new BuildingId(dbEntity.Id));

            return building.Id;
        }

        public string GetConnectionString(BuildingId buildingId)
        {
            return GetById(buildingId).ConnectionString;
        }

        internal static Building Convert(Entities.Building dbEntity)
        {
            var mainContractor = dbEntity.Companies.Single(bc => bc.ContractType == ContractType.MainContractor).Company;
            return new Building(
                new BuildingId(dbEntity.Id),
                dbEntity.Name,
                new Company(new CompanyId(mainContractor.Id),
                    mainContractor.Name,
                    mainContractor.Nip,
                    mainContractor.PhoneNumber,
                    mainContractor.EMail,
                    mainContractor.City,
                    mainContractor.PostCode,
                    mainContractor.Street,
                    mainContractor.PlaceNumber),
                dbEntity.Companies.Where(c => c.ContractType != ContractType.MainContractor).Select(c => new Company(new CompanyId(c.Company.Id),
                    c.Company.Name,
                    c.Company.Nip,
                    c.Company.PhoneNumber,
                    c.Company.EMail,
                    c.Company.City,
                    c.Company.PostCode,
                    c.Company.Street,
                    c.Company.PlaceNumber)));
        }

        private Entities.Building GetById(BuildingId id)
            => _context.Buildings.Include(b => b.Companies).ThenInclude(c => c.Company).FirstOrDefault(b => b.Id == id.Value);
    }
}