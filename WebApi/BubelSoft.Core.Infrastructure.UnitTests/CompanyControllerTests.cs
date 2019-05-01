using System.Collections.Generic;
using BubelSoft.Core.Domain.Models;
using BubelSoft.Core.Infrastructure.Database;
using BubelSoft.Core.Infrastructure.Database.Repositories.Interfaces;
using BubelSoft.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;
using WebApi.Controllers.Core;

namespace BubelSoft.Core.Infrastructure.UnitTests
{
    [TestFixture]
    public class CompanyControllerTests
    {
        private CompanyController _companyController;
        private ICompanyRepository _companyRepository;
        private IUserRepository _userRepository;
        private IUserSession _userSession;
        private User _user;

        [SetUp]
        public void SetUp()
        {
            _companyRepository = Substitute.For<ICompanyRepository>();
            _userRepository = Substitute.For<IUserRepository>();

            _userSession = Substitute.For<IUserSession>();
            _user = new User(new UserId(12), "cur", "rent", UserCompanyRole.Worker, "current@email.com");
            _userSession.User.Returns(_user);
            var context = new MainContext(new DbContextOptions<MainContext>());
            _companyController = new CompanyController(_userRepository, _companyRepository, _userSession, context);
        }

        [Test]
        public void Get_NotFound_CompanyNotExists()
        {
            var companyId = new CompanyId(12);
            _companyRepository.Get(companyId).ReturnsNull();

            var response = _companyController.Get(companyId.Value);

            Assert.That(response, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public void Get_Forbid_UserHasNoAccess()
        {
            var companyId = new CompanyId(12);
            _companyRepository.Get(companyId).Returns(new Company(companyId, "name", "number"));
            _user.From(new CompanyId(15));

            var response = _companyController.Get(companyId.Value);

            Assert.That(response, Is.TypeOf<ForbidResult>());
        }

        [Test]
        public void Get_Ok_CompanyNotExists()
        {
            var companyId = new CompanyId(12);
            const string name = "name";
            const string number = "number";
            _companyRepository.Get(companyId).Returns(new Company(companyId, name, number));
            _user.From(companyId);
            var response = _companyController.Get(companyId.Value);

            Assert.That(response, Is.TypeOf<OkObjectResult>());
            var result = (response as ObjectResult).Value as WebApi.Controllers.DTO.Core.CompanyResponse;
            Assert.That(result.Id, Is.EqualTo(companyId.Value));
            Assert.That(result.Name, Is.EqualTo(name));
            Assert.That(result.Number, Is.EqualTo(number));
        }

        //[Test]
        //public void Edit_NotFound_CompanyNotExists()
        //{
        //    var companyId = new CompanyId(12);
        //    _companyRepository.Get(companyId).ReturnsNull();

        //    var response = _companyController.Edit(companyId.Value, new CompanyInfo());

        //    Assert.That(response, Is.TypeOf<NotFoundResult>());
        //}

        //[Test]
        //public void Edit_Forbid_UserHasNoAccess()
        //{
        //    var companyId = new CompanyId(12);
        //    _companyRepository.Get(companyId).Returns(new Company(companyId, "cName"));

        //    var user = new User(new UserId(2), "","","", UserCompanyRole.Admin, "","");
        //    user.From(new CompanyId(14));
        //    _userSession.User.Returns(user);

        //    var response = _companyController.Edit(companyId.Value, new CompanyInfo());

        //    Assert.That(response, Is.TypeOf<ForbidResult>());
        //}

        //[Test]
        //public void Edit_Ok_CompanyExistsAndUserHasAccess()
        //{
        //    var companyId = new CompanyId(12);
        //    _companyRepository.Get(companyId).Returns(new Company(companyId, "cName"));

        //    var user = new User(new UserId(2), "","","", UserCompanyRole.Admin, "","");
        //    user.From(companyId);
        //    _userSession.User.Returns(user);

        //    var response = _companyController.Edit(companyId.Value, new CompanyInfo());

        //    Assert.That(response, Is.TypeOf<OkResult>());
        //}

        //[Test]
        //public void AddWorker_NotFound_CompanyNotExists()
        //{
        //    var companyId = new CompanyId(12);
        //    _companyRepository.Get(companyId).ReturnsNull();

        //    var response = _companyController.AddWorker(companyId.Value, new Controllers.DTO.User());

        //    Assert.That(response, Is.TypeOf<NotFoundResult>());
        //}

        //[Test]
        //public void AddWorker_Forbid_UserWorksInOtherCompany()
        //{
        //    var companyId = new CompanyId(12);
        //    _companyRepository.Get(companyId).Returns(new Company(companyId, "cName"));

        //    var user = new User(new UserId(2), "","","", UserCompanyRole.UserAdmin, "","");
        //    user.From(new CompanyId(14));
        //    _userSession.User.Returns(user);

        //    var response = _companyController.AddWorker(companyId.Value, new Controllers.DTO.User());

        //    Assert.That(response, Is.TypeOf<ForbidResult>());
        //}

        //[Test]
        //public void AddWorker_Forbid_UserHasNoRole()
        //{
        //    var companyId = new CompanyId(12);
        //    _companyRepository.Get(companyId).Returns(new Company(companyId, "cName"));

        //    var user = new User(new UserId(2), "","","", UserCompanyRole.Worker, "","");
        //    user.From(new CompanyId(14));
        //    _userSession.User.Returns(user);

        //    var response = _companyController.AddWorker(companyId.Value, new Controllers.DTO.User());

        //    Assert.That(response, Is.TypeOf<ForbidResult>());
        //}

        //[Test]
        //public void AddWorker_Ok_CompanyExistsAndUserHasAccess()
        //{
        //    var companyId = new CompanyId(12);
        //    _companyRepository.Get(companyId).Returns(new Company(companyId, "cName"));

        //    var user = new User(new UserId(2), "","","", UserCompanyRole.Admin, "","");
        //    user.From(companyId);
        //    _userSession.User.Returns(user);

        //    var response = _companyController.AddWorker(companyId.Value, new Controllers.DTO.User());

        //    Assert.That(response, Is.TypeOf<OkObjectResult>());
        //}

        //[Test]
        //public void DeleteWorkers_NotFound_CompanyNotExists()
        //{
        //    var companyId = new CompanyId(12);
        //    _companyRepository.Exists(companyId).Returns(false);

        //    var response = _companyController.DeleteWorkers(companyId.Value, new List<int>());

        //    Assert.That(response, Is.TypeOf<NotFoundResult>());
        //}

        //[Test]
        //public void DeleteWorkers_Forbid_UserWorksInOtherCompany()
        //{
        //    var companyId = new CompanyId(12);
        //    _companyRepository.Exists(companyId).Returns(true);

        //    var user = new User(new UserId(2), "", "", "", UserCompanyRole.UserAdmin, "", "");
        //    user.From(new CompanyId(14));
        //    _userSession.User.Returns(user);

        //    var response = _companyController.DeleteWorkers(companyId.Value, new List<int>());

        //    Assert.That(response, Is.TypeOf<ForbidResult>());
        //}

        //[Test]
        //public void DeleteWorkers_Forbid_UserHasNoRole()
        //{
        //    var companyId = new CompanyId(12);
        //    _companyRepository.Exists(companyId).Returns(true);

        //    var user = new User(new UserId(2), "", "", "", UserCompanyRole.Worker, "", "");
        //    user.From(companyId);
        //    _userSession.User.Returns(user);

        //    var response = _companyController.DeleteWorkers(companyId.Value, new List<int>());

        //    Assert.That(response, Is.TypeOf<ForbidResult>());
        //}

        //[Test]
        //public void DeleteWorkers_Ok_CompanyExistsAndUserHasAccess()
        //{
        //    var companyId = new CompanyId(12);
        //    _companyRepository.Exists(companyId).Returns(true);

        //    var user = new User(new UserId(2), "", "", "", UserCompanyRole.Admin, "", "");
        //    user.From(companyId);
        //    _userSession.User.Returns(user);

        //    var response = _companyController.DeleteWorkers(companyId.Value, new List<int>());

        //    Assert.That(response, Is.TypeOf<OkResult>());
        //}
    }
}