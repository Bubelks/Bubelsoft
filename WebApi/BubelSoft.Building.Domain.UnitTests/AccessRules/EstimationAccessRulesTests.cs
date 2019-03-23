using BubelSoft.Building.Domain.AccessRules;
using BubelSoft.Building.Domain.Models;
using BubelSoft.Core.Domain.Models;
using BubelSoft.Core.Infrastructure.Database.Repositories.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace BubelSoft.Building.Domain.UnitTests.AccessRules
{
    [TestFixture]
    public class EstimationAccessRulesTests
    {
        private EstimationAccessRules _estimationAccessRule;
        private IUserRepository _userRepository;
        private IBuildingRepository _buildingRepository;

        [SetUp]
        public void SetUp()
        {
            _userRepository = Substitute.For<IUserRepository>();
            _buildingRepository = Substitute.For<IBuildingRepository>();
            _estimationAccessRule = new EstimationAccessRules(_userRepository, _buildingRepository);
        }

        [Test]
        public void CanEdit_True_UserIsAdminAndWorksInMainContractor()
        {
            var estimation = Estimation.Existing(12, "", "", "", "", 1, 1, 1, new CompanyId(12));
            var userId = new UserId(2);
            var buildingId = new BuildingId(4);
            var mainContractorId = new CompanyId(14);

            var user = new User(userId, "", "", "", UserCompanyRole.Worker, "", "");
            user.From(mainContractorId);
            user.AddRole(buildingId, UserBuildingRole.Admin);
            _userRepository.Get(userId).Returns(user);

            var building = new Core.Domain.Models.Building(buildingId, "name", new Company(mainContractorId, "cName"));
            _buildingRepository.Get(buildingId).Returns(building);

            var canEdit = _estimationAccessRule.CanEdit(estimation, userId, buildingId);

            Assert.IsTrue(canEdit);
        }

        [Test]
        public void CanEdit_False_UserIsAdminButNotWorksInMainContractor()
        {
            var estimation = Estimation.Existing(12, "", "", "", "", 1, 1, 1, new CompanyId(12));
            var userId = new UserId(2);
            var buildingId = new BuildingId(4);
            var mainContractorId = new CompanyId(14);

            var user = new User(userId, "", "", "", UserCompanyRole.Worker, "", "");
            user.From(new CompanyId(12));
            user.AddRole(buildingId, UserBuildingRole.Admin);
            _userRepository.Get(userId).Returns(user);

            var building = new Core.Domain.Models.Building(buildingId, "name", new Company(mainContractorId, "cName"));
            _buildingRepository.Get(buildingId).Returns(building);

            var canEdit = _estimationAccessRule.CanEdit(estimation, userId, buildingId);

            Assert.IsFalse(canEdit);
        }

        [Test]
        public void CanEdit_False_UserIsNotAdminAndWorksInMainContractor()
        {
            var estimation = Estimation.Existing(12, "", "", "", "", 1, 1, 1, new CompanyId(12));
            var userId = new UserId(2);
            var buildingId = new BuildingId(4);
            var mainContractorId = new CompanyId(14);

            var user = new User(userId, "", "", "", UserCompanyRole.Worker, "", "");
            user.From(mainContractorId);
            user.AddRole(buildingId, UserBuildingRole.Supervisor);
            user.AddRole(buildingId, UserBuildingRole.Reporter);
            _userRepository.Get(userId).Returns(user);

            var building = new Core.Domain.Models.Building(buildingId, "name", new Company(mainContractorId, "cName"));
            _buildingRepository.Get(buildingId).Returns(building);

            var canEdit = _estimationAccessRule.CanEdit(estimation, userId, buildingId);

            Assert.IsFalse(canEdit);
        }
    }
}