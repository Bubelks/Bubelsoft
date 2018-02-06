using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebApi.Database.Entities;
using WebApi.Database.Repositories.Interfaces;
using WebApi.Domain.Models;
using Company = WebApi.Domain.Models.Company;

namespace WebApi.Database.Repositories
{
    internal class CompanyRepository : ICompanyRepository
    {
        private readonly MainContext _context;

        public CompanyRepository(MainContext context)
        {
            _context = context;
        }

        public CompanyId Save(Company company)
        {
            var entity = GetById(company.Id.Value) ?? new Entities.Company();

            entity.Name = company.Name;
            entity.Nip = company.Nip;
            entity.PhoneNumber = company.PhoneNumber;
            entity.EMail = company.Email;
            entity.City = company.City;
            entity.PostCode = company.PostCode;
            entity.Street = company.Street;
            entity.PlaceNumber = company.PlaceNumber;

            if (entity.Id == 0)
                _context.Companies.Add(entity);
            _context.SaveChanges();

            if (company.Id.Value == 0)
                company.SetId(new CompanyId(entity.Id));

            return company.Id;
        }

        public Company Get(CompanyId companyId)
        {
            var entity = GetById(companyId.Value);

            if (entity == null)
                return null;

            return new Company(companyId,
                entity.Name,
                entity.Nip,
                entity.PhoneNumber,
                entity.EMail,
                entity.City,
                entity.PostCode,
                entity.Street,
                entity.PlaceNumber);
        }

        public bool Exists(CompanyId companyId)
        {
            return _context.Companies.Any(c => c.Id == companyId.Value);
        }

        public void DeleteWorkers(CompanyId companyId, IEnumerable<UserId> users)
        {
            var usersEntities = _context.Users
                .Where(u => users.Any(uId => uId.Value == u.Id))
                .Where(u => u.CompanyId == companyId.Value);

            usersEntities.ForEachAsync(u =>
            {
                u.CompanyId = null;
                u.Company = null;
            });

            _context.SaveChanges();
        }

        public IEnumerable<Company> GetContractors(BuildingId buildingId)
        {
            return _context.Companies
                .Where(c => c.Buildings.Any(bc => bc.BuildingId == buildingId.Value))
                .Select(entity => new Company(new CompanyId(entity.Id),
                    entity.Name,
                    entity.Nip,
                    entity.PhoneNumber,
                    entity.EMail,
                    entity.City,
                    entity.PostCode,
                    entity.Street,
                    entity.PlaceNumber));
        }

        public IEnumerable<Company> GetAll()
        {
            return _context.Companies
                .Select(entity => new Company(new CompanyId(entity.Id),
                    entity.Name,
                    entity.Nip,
                    entity.PhoneNumber,
                    entity.EMail,
                    entity.City,
                    entity.PostCode,
                    entity.Street,
                    entity.PlaceNumber));
        }

        public void AddSubContract(BuildingId buildingId, CompanyId companyId)
        {
            var company = _context.Companies.Single(c => c.Id == companyId.Value);
            var admin = _context.Users.FirstOrDefault(u =>
                u.CompanyId == companyId.Value && u.CompanyRole == UserCompanyRole.Admin);
            if(company.Buildings == null)
                company.Buildings = new List<BuildingCompany>();

            company.Buildings.Add(new BuildingCompany
            {
                BuildingId = buildingId.Value,
                ContractType = ContractType.SubContractor,
                Users = new List<UserRole>(new []
                {
                    new UserRole
                    {
                        UserId = admin.Id,
                        UserBuildingRole = UserBuildingRole.Admin
                    }
                })
            });

            _context.SaveChanges();
        }

        private Entities.Company GetById(int id)
            => _context.Companies.FirstOrDefault(c => c.Id == id);
    }
}