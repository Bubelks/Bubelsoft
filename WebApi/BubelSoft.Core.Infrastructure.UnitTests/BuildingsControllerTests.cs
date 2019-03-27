using BubelSoft.Core.Domain.Models;
using BubelSoft.Core.Infrastructure.Controllers;
using BubelSoft.Core.Infrastructure.Database.Repositories.Interfaces;
using BubelSoft.Core.Infrastructure.Services;
using BubelSoft.Security;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;
using User = BubelSoft.Core.Domain.Models.User;

namespace BubelSoft.Core.Infrastructure.UnitTests
{
    [TestFixture]
    public class BuildingsControllerTests
    {
        private BuildingsController _buildingsController;
        private IBuildingRepository _buildingRepository;
        private ICompanyRepository _companyRepository;
        private IUserSession _userSession;
        private IUserRepository _userRepository;
        private IMailService _mailService;

        [SetUp]
        public void SetUp()
        {
            _buildingRepository = Substitute.For<IBuildingRepository>();
            _companyRepository = Substitute.For<ICompanyRepository>();
            _userSession = Substitute.For<IUserSession>();
            _userRepository = Substitute.For<IUserRepository>();
            _mailService = Substitute.For<IMailService>();

            _buildingsController = new BuildingsController(_buildingRepository, _companyRepository, _userSession,
                _userRepository, _mailService);
        }

        [Test]
        public void Get_NotFound_BuildingNotExists()
        {
            var buildingId = new BuildingId(12);
            _buildingRepository.Get(buildingId).ReturnsNull();

            var response = _buildingsController.Get(buildingId.Value);

            Assert.That(response, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public void Get_Forbid_UserHasNoAccess()
        {
            var buildingId = new BuildingId(12);
            var companyId = new CompanyId(16);
            var building = new Building(buildingId, "name", new Company(companyId, "cName"));
            _buildingRepository.Get(buildingId).Returns(building);

            var user = new User(new UserId(2), "", "", "", UserCompanyRole.UserAdmin, "", "");
            user.From(companyId);
            _userSession.User.Returns(user);

            var response = _buildingsController.Get(buildingId.Value);

            Assert.That(response, Is.TypeOf<ForbidResult>());
        }

        [Test]
        public void Get_Ok_BuildingExistsUserHasAccess()
        {
            var buildingId = new BuildingId(12);
            var companyId = new CompanyId(16);
            var building = new Building(buildingId, "name", new Company(companyId, "cName"));
            _buildingRepository.Get(buildingId).Returns(building);

            var user = new User(new UserId(2), "", "", "", UserCompanyRole.UserAdmin, "", "");
            user.From(companyId);
            user.AddRole(buildingId, UserBuildingRole.Supervisor);
            _userSession.User.Returns(user);

            var response = _buildingsController.Get(buildingId.Value);

            Assert.That(response, Is.TypeOf<OkObjectResult>());
        }

        [Test]
        public void GetCompanies_NotFound_BuildingNotExists()
        {
            var buildingId = new BuildingId(12);
            _buildingRepository.Get(buildingId).ReturnsNull();

            var response = _buildingsController.GetCompanies(buildingId.Value);

            Assert.That(response, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public void GetCompanies_Forbid_UserHasNoAccess()
        {
            var buildingId = new BuildingId(12);
            var companyId = new CompanyId(16);
            var building = new Building(buildingId, "name", new Company(companyId, "cName"));
            _buildingRepository.Get(buildingId).Returns(building);

            var user = new User(new UserId(2), "", "", "", UserCompanyRole.UserAdmin, "", "");
            user.From(companyId);
            _userSession.User.Returns(user);

            var response = _buildingsController.GetCompanies(buildingId.Value);

            Assert.That(response, Is.TypeOf<ForbidResult>());
        }

        [Test]
        public void GetCompanies_Ok_BuildingExistsUserHasAccess()
        {
            var buildingId = new BuildingId(12);
            var companyId = new CompanyId(16);
            var building = new Building(buildingId, "name", new Company(companyId, "cName"));
            _buildingRepository.Get(buildingId).Returns(building);

            var user = new User(new UserId(2), "", "", "", UserCompanyRole.UserAdmin, "", "");
            user.From(companyId);
            user.AddRole(buildingId, UserBuildingRole.Supervisor);
            _userSession.User.Returns(user);

            var response = _buildingsController.GetCompanies(buildingId.Value);

            Assert.That(response, Is.TypeOf<OkObjectResult>());
        }
    }
}