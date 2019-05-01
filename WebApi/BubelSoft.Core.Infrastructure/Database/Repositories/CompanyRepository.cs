using System.Collections.Generic;
using System.Linq;
using BubelSoft.Core.Domain.Models;
using BubelSoft.Core.Infrastructure.Database.Entities;
using BubelSoft.Core.Infrastructure.Database.Repositories.Interfaces;
using Company = BubelSoft.Core.Domain.Models.Company;

namespace BubelSoft.Core.Infrastructure.Database.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly MainContext _context;

        public CompanyRepository(MainContext context)
        {
            _context = context;
        }

        public CompanyId Save(Company company)
        {
            var entity = company.IsNew 
                ? new Entities.Company()
                : GetById(company.Id.Value);

            entity.Name = company.Name;
            entity.Number = company.Number;

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
                entity.Number);
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

            foreach (var usersEntity in usersEntities)
            {
                usersEntity.CompanyId = null;
                usersEntity.Company = null;
            }

            _context.SaveChanges();
        }

        public IEnumerable<Company> GetContractors(BuildingId buildingId)
        {
            return _context.Companies
                .Where(c => c.Buildings.Any(bc => bc.BuildingId == buildingId.Value))
                .Select(entity => new Company(new CompanyId(entity.Id),
                    entity.Name,
                    entity.Number));
        }

        public IEnumerable<Company> GetAll()
        {
            return _context.Companies
                .Select(entity => new Company(new CompanyId(entity.Id),
                    entity.Name,
                    entity.Number));
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