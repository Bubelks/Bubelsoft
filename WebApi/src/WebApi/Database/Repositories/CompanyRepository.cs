using System.Linq;
using WebApi.Database.Repositories.Interfaces;
using WebApi.Domain.Models;

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
            var entity = GetById(company.Id.Value) ?? new Entities.Company
            {
                Name = company.Name
            };

            entity.Name = company.Name;

            if (entity.Id == 0)
                _context.Companies.Add(entity);
            _context.SaveChanges();

            company.SetId(new CompanyId(entity.Id));
            return company.Id;
        }

        private Entities.Company GetById(int id)
            => _context.Companies.FirstOrDefault(c => c.Id == id);
    }
}