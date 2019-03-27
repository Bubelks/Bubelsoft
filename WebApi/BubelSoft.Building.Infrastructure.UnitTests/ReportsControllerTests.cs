using System;
using System.Collections.Generic;
using BubelSoft.Building.Domain.AccessRules;
using BubelSoft.Building.Infrastructure;
using BubelSoft.Building.Infrastructure.Controllers;
using BubelSoft.Building.Infrastructure.Repositories;
using BubelSoft.Core.Domain.Models;
using BubelSoft.Core.Infrastructure;
using BubelSoft.Core.Infrastructure.Database.Repositories.Interfaces;
using BubelSoft.Security;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;
using Report = BubelSoft.Building.Domain.Models.Report;

namespace BubelSoft.Building.Infrasturcture.UnitTests
{
    [TestFixture]
    public class ReportsControllerTests
    {
        private ReportsController _reportsController;
        private IBuildingRepository _buildingRepository;
        private IUserSession _userSession;
        private IUserRepository _userRepository;
        private ICompanyRepository _companyRepository;
        private IRepositoryFactory _repositoryFactory;
        private IReportRepository _reportsRepository;
        private IEstimationRepository _estimationRepository;
        private IReportAccessRules _reportAccessRules;

        [SetUp]
        public void SetUp()
        {
            _userSession = Substitute.For<IUserSession>();

            _buildingRepository = Substitute.For<IBuildingRepository>();
            _userRepository = Substitute.For<IUserRepository>();
            _companyRepository = Substitute.For<ICompanyRepository>();

            _repositoryFactory = Substitute.For<IRepositoryFactory>();
            _reportsRepository = Substitute.For<IReportRepository>();
            _estimationRepository = Substitute.For<IEstimationRepository>();
            _repositoryFactory.Report(Arg.Any<string>()).Returns(_reportsRepository);
            _repositoryFactory.Estimation(Arg.Any<string>()).Returns(_estimationRepository);

            _reportAccessRules = Substitute.For<IReportAccessRules>();
            _reportsController =
                new ReportsController(_buildingRepository, _userSession, _userRepository, _companyRepository, _repositoryFactory, _reportAccessRules);
        }

        [Test]
        public void GetReport_NotFound_ReportIsNotExists()
        {
            const int reportId = 19;

            _reportsRepository.Get(reportId, Arg.Any<BuildingId>()).ReturnsNull();

            var response = _reportsController.GetReport(12, reportId);

            Assert.That(response, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public void GetReport_Forbidden_ReportExistsButUserHasNoAccess()
        {
            const int reportId = 19;
            var report = new Report(reportId, DateTime.Now, 1, new UserId(1), new BuildingId(2));

            _reportsRepository.Get(reportId, Arg.Any<BuildingId>()).Returns(report);
            _reportAccessRules.CanAccess(report, Arg.Any<UserId>()).Returns(false);

            var response = _reportsController.GetReport(12, reportId);

            Assert.That(response, Is.TypeOf<ForbidResult>());
        }

        [Test]
        public void GetReport_Ok_ReportIfReportsExistsAnd()
        {
            const int reportId = 19;
            var report = new Report(reportId, DateTime.Now, 1, new UserId(1), new BuildingId(2));

            _reportsRepository.Get(reportId, Arg.Any<BuildingId>()).Returns(report);
            _reportAccessRules.CanAccess(report, Arg.Any<UserId>()).Returns(true);

            var response = _reportsController.GetReport(12, reportId);

            Assert.That(response, Is.TypeOf<OkObjectResult>());
        }

        [Test]
        public void UpdateReport_NotFound_ReportIsNotExists()
        {
            const int reportId = 19;

            _reportsRepository.Exists(reportId).Returns(false);

            var response = _reportsController.UpdateReport(12, reportId, new ReportDTO
            {
                Date = DateTime.UtcNow,
                NumberOfWorkers = 12,
                Work = new List<ReportQuantity>(),
                CanEdit = false
            });

            Assert.That(response, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public void UpdateReport_Forbidden_ReportExistsButUserHasNoAccess()
        {
            const int reportId = 19;

            _reportsRepository.Exists(reportId).Returns(true);
            _reportAccessRules.CanAccess(Arg.Any<Report>(), Arg.Any<UserId>()).Returns(false);

            var response = _reportsController.UpdateReport(12, reportId, new ReportDTO
            {
                Date = DateTime.UtcNow,
                NumberOfWorkers = 12,
                Work = new List<ReportQuantity>(),
                CanEdit = false
            });


            Assert.That(response, Is.TypeOf<ForbidResult>());
        }

        [Test]
        public void UpdateReport_Ok_ReportIfReportsExistsAnd()
        {
            const int reportId = 19;

            _reportsRepository.Exists(reportId).Returns(true);
            _reportAccessRules.CanAccess(Arg.Any<Report>(), Arg.Any<UserId>()).Returns(true);

            var response = _reportsController.UpdateReport(12, reportId, new ReportDTO
            {
                Date = DateTime.UtcNow,
                NumberOfWorkers = 12,
                Work = new List<ReportQuantity>(),
                CanEdit = false
            });

            Assert.That(response, Is.TypeOf<OkResult>());
        }
    }
}
