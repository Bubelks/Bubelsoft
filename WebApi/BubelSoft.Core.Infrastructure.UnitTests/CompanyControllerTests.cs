using System;
using System.Collections.Generic;
using BubelSoft.Core.Domain.Models;
using BubelSoft.Core.Infrastructure.Controllers;
using BubelSoft.Core.Infrastructure.Controllers.DTO;
using BubelSoft.Core.Infrastructure.Database.Repositories.Interfaces;
using BubelSoft.Core.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;
using Company = BubelSoft.Core.Domain.Models.Company;
using User = BubelSoft.Core.Domain.Models.User;

namespace BubelSoft.Core.Infrastructure.UnitTests
{
    [TestFixture]
    public class CompanyControllerTests
    {
        private CompanyController _companyController;
        private ICompanyRepository _companyRepository;
        private IUserRepository _userRepository;
        private ICurrentUser _currentUser;
        private IMailService _mailService;

        [SetUp]
        public void SetUp()
        {
            _companyRepository = Substitute.For<ICompanyRepository>();
            _userRepository = Substitute.For<IUserRepository>();
            _currentUser = Substitute.For<ICurrentUser>();
            _mailService = Substitute.For<IMailService>();

            _companyController = new CompanyController(_companyRepository, _userRepository, _currentUser, _mailService);
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
        public void Get_Ok_CompanyNotExists()
        {
            var companyId = new CompanyId(12);
            _companyRepository.Get(companyId).Returns(new Company(companyId, "name"));
            _userRepository.GetWorkers(companyId).Returns(new List<User>());
            _currentUser.User.Returns(new User(new UserId(13), "", "", "", UserCompanyRole.Admin, "", ""));

            var response = _companyController.Get(companyId.Value);

            Assert.That(response, Is.TypeOf<OkObjectResult>());
        }

        [Test]
        public void Edit_NotFound_CompanyNotExists()
        {
            var companyId = new CompanyId(12);
            _companyRepository.Get(companyId).ReturnsNull();

            var response = _companyController.Edit(companyId.Value, new CompanyInfo());

            Assert.That(response, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public void Edit_Forbid_UserHasNoAccess()
        {
            var companyId = new CompanyId(12);
            _companyRepository.Get(companyId).Returns(new Company(companyId, "cName"));

            var user = new User(new UserId(2), "","","", UserCompanyRole.Admin, "","");
            user.From(new CompanyId(14));
            _currentUser.User.Returns(user);

            var response = _companyController.Edit(companyId.Value, new CompanyInfo());

            Assert.That(response, Is.TypeOf<ForbidResult>());
        }

        [Test]
        public void Edit_Ok_CompanyExistsAndUserHasAccess()
        {
            var companyId = new CompanyId(12);
            _companyRepository.Get(companyId).Returns(new Company(companyId, "cName"));

            var user = new User(new UserId(2), "","","", UserCompanyRole.Admin, "","");
            user.From(companyId);
            _currentUser.User.Returns(user);

            var response = _companyController.Edit(companyId.Value, new CompanyInfo());

            Assert.That(response, Is.TypeOf<OkResult>());
        }

        [Test]
        public void AddWorker_NotFound_CompanyNotExists()
        {
            var companyId = new CompanyId(12);
            _companyRepository.Get(companyId).ReturnsNull();

            var response = _companyController.AddWorker(companyId.Value, new Controllers.DTO.User());

            Assert.That(response, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public void AddWorker_Forbid_UserWorksInOtherCompany()
        {
            var companyId = new CompanyId(12);
            _companyRepository.Get(companyId).Returns(new Company(companyId, "cName"));

            var user = new User(new UserId(2), "","","", UserCompanyRole.UserAdmin, "","");
            user.From(new CompanyId(14));
            _currentUser.User.Returns(user);

            var response = _companyController.AddWorker(companyId.Value, new Controllers.DTO.User());

            Assert.That(response, Is.TypeOf<ForbidResult>());
        }

        [Test]
        public void AddWorker_Forbid_UserHasNoRole()
        {
            var companyId = new CompanyId(12);
            _companyRepository.Get(companyId).Returns(new Company(companyId, "cName"));

            var user = new User(new UserId(2), "","","", UserCompanyRole.Worker, "","");
            user.From(new CompanyId(14));
            _currentUser.User.Returns(user);

            var response = _companyController.AddWorker(companyId.Value, new Controllers.DTO.User());

            Assert.That(response, Is.TypeOf<ForbidResult>());
        }

        [Test]
        public void AddWorker_Ok_CompanyExistsAndUserHasAccess()
        {
            var companyId = new CompanyId(12);
            _companyRepository.Get(companyId).Returns(new Company(companyId, "cName"));

            var user = new User(new UserId(2), "","","", UserCompanyRole.Admin, "","");
            user.From(companyId);
            _currentUser.User.Returns(user);

            var response = _companyController.AddWorker(companyId.Value, new Controllers.DTO.User());

            Assert.That(response, Is.TypeOf<OkObjectResult>());
        }

        [Test]
        public void DeleteWorkers_NotFound_CompanyNotExists()
        {
            var companyId = new CompanyId(12);
            _companyRepository.Exists(companyId).Returns(false);

            var response = _companyController.DeleteWorkers(companyId.Value, new List<int>());

            Assert.That(response, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public void DeleteWorkers_Forbid_UserWorksInOtherCompany()
        {
            var companyId = new CompanyId(12);
            _companyRepository.Exists(companyId).Returns(true);

            var user = new User(new UserId(2), "", "", "", UserCompanyRole.UserAdmin, "", "");
            user.From(new CompanyId(14));
            _currentUser.User.Returns(user);

            var response = _companyController.DeleteWorkers(companyId.Value, new List<int>());

            Assert.That(response, Is.TypeOf<ForbidResult>());
        }

        [Test]
        public void DeleteWorkers_Forbid_UserHasNoRole()
        {
            var companyId = new CompanyId(12);
            _companyRepository.Exists(companyId).Returns(true);

            var user = new User(new UserId(2), "", "", "", UserCompanyRole.Worker, "", "");
            user.From(companyId);
            _currentUser.User.Returns(user);

            var response = _companyController.DeleteWorkers(companyId.Value, new List<int>());

            Assert.That(response, Is.TypeOf<ForbidResult>());
        }

        [Test]
        public void DeleteWorkers_Ok_CompanyExistsAndUserHasAccess()
        {
            var companyId = new CompanyId(12);
            _companyRepository.Exists(companyId).Returns(true);

            var user = new User(new UserId(2), "", "", "", UserCompanyRole.Admin, "", "");
            user.From(companyId);
            _currentUser.User.Returns(user);

            var response = _companyController.DeleteWorkers(companyId.Value, new List<int>());

            Assert.That(response, Is.TypeOf<OkResult>());
        }
    }
}