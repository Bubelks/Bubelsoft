using System.Linq;
using BubelSoft.Core.Domain.Models;
using BubelSoft.Core.Infrastructure.Database;
using BubelSoft.Core.Infrastructure.Database.Repositories;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace BubelSoft.Core.Infrastructure.UnitTests
{
    [TestFixture]
    public class UserRepositoryTest
    {
        private UserRepository _userRepository;
        private MainContext _context;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<MainContext>()
                .UseInMemoryDatabase(databaseName: "InMemory")
                .Options;

            _context = new MainContext(options);
            _userRepository = new UserRepository(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
        }

        [Test]
        public void Save_Create_ForNewCompany()
        {
            var user = new User("firstName", "lastName", UserCompanyRole.Worker, "email");
            var oldId = user.Id;

            var newId = _userRepository.Save(user);

            Assert.That(newId, Is.Not.EqualTo(oldId), "Changed id");
            Assert.That(_context.Users.Count(), Is.EqualTo(1), "Count");

            var addedUser = _context.Users.Single();
            Assert.That(addedUser.FirstName, Is.EqualTo(user.FirstName), "First name");
            Assert.That(addedUser.LastName, Is.EqualTo(user.LastName), "Last name");
            Assert.That(addedUser.CompanyRole, Is.EqualTo(user.CompanyRole), "Company role");
            Assert.That(addedUser.Email, Is.EqualTo(user.Email), "Email");
            Assert.That(addedUser.Id, Is.EqualTo(user.Id.Value), "Id");
        }

        [Test]
        public void Save_Update_ForExistingCompany()
        {
            var user = new User(new UserId(18), "firstName", "lastName", UserCompanyRole.Worker, "email");
            var oldId = user.Id;
            _context.Users.Add(
                new Database.Entities.User
                {
                    Id = user.Id.Value,
                    FirstName = "oldName",
                    LastName = "oldNumber",
                    CompanyRole = UserCompanyRole.UserAdmin,
                    Email = "oldEmail",
                });
            _context.SaveChanges();

            var newId = _userRepository.Save(user);

            Assert.That(newId, Is.EqualTo(oldId), "Changed id");
            Assert.That(_context.Users.Count(), Is.EqualTo(1), "Count");

            var addedUser = _context.Users.Single();
            Assert.That(addedUser.FirstName, Is.EqualTo(user.FirstName), "First name");
            Assert.That(addedUser.LastName, Is.EqualTo(user.LastName), "Last name");
            Assert.That(addedUser.CompanyRole, Is.EqualTo(user.CompanyRole), "Company role");
            Assert.That(addedUser.Email, Is.EqualTo(user.Email), "Email");
            Assert.That(addedUser.Id, Is.EqualTo(user.Id.Value), "Id");
        }
    }
}
