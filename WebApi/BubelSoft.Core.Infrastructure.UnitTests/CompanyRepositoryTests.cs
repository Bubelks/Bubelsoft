using System.Linq;
using BubelSoft.Core.Domain.Models;
using BubelSoft.Core.Infrastructure.Database;
using BubelSoft.Core.Infrastructure.Database.Repositories;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace BubelSoft.Core.Infrastructure.UnitTests
{
    [TestFixture]
    public class CompanyRepositoryTests
    {
        private CompanyRepository _companyRepository;
        private MainContext _context;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<MainContext>()
                .UseInMemoryDatabase(databaseName: "InMemory")
                .Options;

            _context = new MainContext(options);
            _companyRepository = new CompanyRepository(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
        }

        [Test]
        public void Save_Create_ForNewCompany()
        {
            var company = new Company("newCompany", "newNumber");
            var oldId = company.Id;

            var newId =  _companyRepository.Save(company);

            Assert.That(newId, Is.Not.EqualTo(oldId), "Changed id");
            Assert.That(_context.Companies.Count(), Is.EqualTo(1), "Count");

            var addedCompany = _context.Companies.Single();
            Assert.That(addedCompany.Name, Is.EqualTo(company.Name), "Company name");
            Assert.That(addedCompany.Number, Is.EqualTo(company.Number), "Company number");
            Assert.That(addedCompany.Id, Is.EqualTo(company.Id.Value), "Company id");
        }

        [Test]
        public void Save_Update_ForExistingCompany()
        {
            var company = new Company(new CompanyId(17), "newCompany", "newNumber");
            var oldId = company.Id;
            _context.Companies.Add(
                new Database.Entities.Company{
                    Id = company.Id.Value,
                    Name = "oldName",
                    Number = "oldNumber"
                });
            _context.SaveChanges();

            var newId = _companyRepository.Save(company);

            Assert.That(newId, Is.EqualTo(oldId), "Changed id");
            Assert.That(_context.Companies.Count(), Is.EqualTo(1), "Count");

            var updatedCompany = _context.Companies.Single();
            Assert.That(updatedCompany.Name, Is.EqualTo(company.Name), "Company name");
            Assert.That(updatedCompany.Number, Is.EqualTo(company.Number), "Company number");
            Assert.That(updatedCompany.Id, Is.EqualTo(company.Id.Value), "Company id");
        }
    }
}
