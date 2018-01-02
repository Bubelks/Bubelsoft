using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebApi.Database.Entities;
using WebApi.Database.Repositories.Interfaces;
using WebApi.Domain.Models;
using Building = WebApi.Domain.Models.Building;
using Company = WebApi.Domain.Models.Company;

namespace WebApi.Database.Repositories
{
    internal class BuildingRepository : IBuildingRepository
    {
        private readonly MainContext _context;

        public BuildingRepository(MainContext context)
        {
            _context = context;
        }

        public IEnumerable<Building> GetFor(UserId userId)
        {
            var enities = _context.Buildings
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
            if(dbEntity.Companies.All(c => c.CompanyId != building.MainContractor.Id.Value))
                dbEntity.Companies.Add(new BuildingCompany
                {
                    CompanyId = building.MainContractor.Id.Value,
                    ContractType = ContractType.MainContractor
                });

            if (dbEntity.Id == 0)
                _context.Buildings.Add(dbEntity);
            _context.SaveChanges();

            if (building.Id.Value == 0)
                building.SetId(new BuildingId(dbEntity.Id));

            return building.Id;
        }

        private static Building Convert(Entities.Building dbEntity)
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
                    mainContractor.PlaceNumber));
        }

        private Entities.Building GetById(BuildingId id)
            => _context.Buildings.Include(b => b.Companies).ThenInclude(c => c.Company).FirstOrDefault(b => b.Id == id.Value);
    }
}