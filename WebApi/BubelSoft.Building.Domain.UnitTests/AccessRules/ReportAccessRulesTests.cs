using System;
using BubelSoft.Building.Domain.AccessRules;
using BubelSoft.Building.Domain.Models;
using BubelSoft.Core.Domain.Models;
using BubelSoft.Core.Infrastructure.Database.Repositories.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace BubelSoft.Building.Domain.UnitTests.AccessRules
{
    [TestFixture]
    public class ReportAccessRulesTests
    {
        private ReportAccessRules _reportAccessRule;
        private IUserRepository _userRepository;
        private IBuildingRepository _buildingRepository;

        [SetUp]
        public void SetUp()
        {
            _userRepository = Substitute.For<IUserRepository>();
            _buildingRepository = Substitute.For<IBuildingRepository>();
            _reportAccessRule = new ReportAccessRules(_userRepository, _buildingRepository);
        }

        [Test]
        public void CanAccess_ReturnTrue_WhenUserIsReporter()
        {
            var userId = new UserId(7);
            var report = new Report(DateTime.Now, 1, userId, new BuildingId(2));

            var access = _reportAccessRule.CanAccess(report, userId);

            Assert.IsTrue(access);
        }

        [Test]
        public void CanAccess_ReturnTrue_WhenUserIsSupervisorOnBuildingInReporterCompany()
        {
            var companyId = new CompanyId(14);
            var buildingId = new BuildingId(16);

            var userId = new UserId(7);
            var user = new User(userId, "", "", UserCompanyRole.Worker, "");
            user.From(companyId);
            user.AddRole(buildingId, UserBuildingRole.Supervisor);
            _userRepository.Get(userId).Returns(user);

            var reporterId = new UserId(5);
            var reporter = new User(userId, "", "", UserCompanyRole.Worker, "");
            reporter.From(companyId);
            _userRepository.Get(reporterId).Returns(reporter);

            var report = new Report(DateTime.Now, 1, reporterId, buildingId);

            var access = _reportAccessRule.CanAccess(report, userId);

            Assert.IsTrue(access);
        }

        [Test]
        public void CanAccess_ReturnTrue_WhenUserIsSupervisorOnMainContractorOfBuilding()
        {
            var mainContractorId = new CompanyId(14);

            var buildingId = new BuildingId(16);
            _buildingRepository.Get(buildingId)
                .Returns(new Core.Domain.Models.Building(buildingId, "name", new Company(mainContractorId, "cName", "cNumber")));

            var userId = new UserId(7);
            var user = new User(userId, "", "", UserCompanyRole.Worker, "");
            user.From(mainContractorId);
            user.AddRole(buildingId, UserBuildingRole.Supervisor);
            _userRepository.Get(userId).Returns(user);

            var reporterId = new UserId(5);
            var reporter = new User(userId, "", "", UserCompanyRole.Worker, "");
            reporter.From(new CompanyId(15));
            _userRepository.Get(reporterId).Returns(reporter);

            var report = new Report(DateTime.Now, 1, reporterId, buildingId);

            var access = _reportAccessRule.CanAccess(report, userId);

            Assert.IsTrue(access);
        }

        [Test]
        public void CanAccess_ReturnFalse_WhenUserIsSupervisorOnOtherSubContractor()
        {
            var buildingId = new BuildingId(16);
            _buildingRepository.Get(buildingId)
                .Returns(new Core.Domain.Models.Building(buildingId, "name", new Company(new CompanyId(11), "cName", "cNumber")));

            var userId = new UserId(7);
            var user = new User(userId, "", "", UserCompanyRole.Worker, "");
            user.From(new CompanyId(14));
            user.AddRole(buildingId, UserBuildingRole.Supervisor);
            _userRepository.Get(userId).Returns(user);

            var reporterId = new UserId(5);
            var reporter = new User(userId, "", "", UserCompanyRole.Worker, "");
            reporter.From(new CompanyId(15));
            _userRepository.Get(reporterId).Returns(reporter);

            var report = new Report(DateTime.Now, 1, reporterId, buildingId);

            var access = _reportAccessRule.CanAccess(report, userId);

            Assert.IsFalse(access);
        }

        [Test]
        public void CanAccess_ReturnFalse_WhenUserIsNotSupervisor()
        {
            var buildingId = new BuildingId(16);
            var userId = new UserId(7);
            var user = new User(userId, "", "", UserCompanyRole.Worker, "");
            _userRepository.Get(userId).Returns(user);

            var reporterId = new UserId(5);
            var report = new Report(DateTime.Now, 1, reporterId, buildingId);

            var access = _reportAccessRule.CanAccess(report, userId);

            Assert.IsFalse(access);
        }
    }
}