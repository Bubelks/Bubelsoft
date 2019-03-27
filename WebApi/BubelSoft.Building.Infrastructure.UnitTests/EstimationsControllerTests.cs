using System;
using System.Collections.Generic;
using BubelSoft.Building.Domain.AccessRules;
using BubelSoft.Building.Infrastructure;
using BubelSoft.Building.Infrastructure.Controllers;
using BubelSoft.Building.Infrastructure.Entities;
using BubelSoft.Building.Infrastructure.Repositories;
using BubelSoft.Core.Domain.Models;
using BubelSoft.Core.Infrastructure.Database.Repositories.Interfaces;
using BubelSoft.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using NUnit.Framework;
using Estimation = BubelSoft.Building.Infrastructure.Controllers.Estimation;
using ReportQuantity = BubelSoft.Building.Infrastructure.Entities.ReportQuantity;

namespace BubelSoft.Building.Infrasturcture.UnitTests
{
    [TestFixture]
    public class EstimationsControllerTests
    {
        private EstimationsController _estimationsController;
        private IBuildingRepository _buildingRepository;
        private ICompanyRepository _companyRepository;
        private IUserRepository _userRepository;
        private IUserSession _userSession;
        private IRepositoryFactory _repositoryFactory;
        private IReportRepository _reportRepository;
        private IEstimationRepository _estimationRepository;
        private IEstimationAccessRules _estimationAccessRules;

        [SetUp]
        public void SetUp()
        {
            _userSession = Substitute.For<IUserSession>();

            _buildingRepository = Substitute.For<IBuildingRepository>();
            _companyRepository = Substitute.For<ICompanyRepository>();
            _userRepository = Substitute.For<IUserRepository>();

            _repositoryFactory = Substitute.For<IRepositoryFactory>();
            _reportRepository = Substitute.For<IReportRepository>();
            _estimationRepository = Substitute.For<IEstimationRepository>();
            _repositoryFactory.Report(Arg.Any<string>()).Returns(_reportRepository);
            _repositoryFactory.Estimation(Arg.Any<string>()).Returns(_estimationRepository);

            _estimationAccessRules = Substitute.For<IEstimationAccessRules>();

            _estimationsController = new EstimationsController(_buildingRepository, _companyRepository, _userRepository, _userSession, _estimationAccessRules,_repositoryFactory);
        }

        [Test]
        public void GenerateEstimation()
        {
            var optionsBuilder = new DbContextOptionsBuilder<BuildingContext>();
            optionsBuilder.UseSqlServer("Server=.;Database=BuildingDatabase;User Id=sa;Password=Bubelsoft1");

            var buildingcontext = new BuildingContext(optionsBuilder.Options);

            var rand = new Random();
            for(var m = 1; m < 13; m++)
            for (var i = 1; i < 15; i++)
            {
                var guid = Guid.NewGuid().ToString().Substring(0, 5);
                var createdDate = DateTime.UtcNow;

                var report = new Report
                {
                    CreatedDate = createdDate,
                    ReportDate = new DateTime(2017, m, rand.Next(1, 27)),
                    WorkersCount = rand.Next(4, 20),
                    ReporterId = 1,
                    Quantities = new List<ReportQuantity>
                    {
                        new ReportQuantity
                        {
                            EstimationId = rand.Next(30000, 100000),
                            Quantity = rand.Next(1, 10)
                        }
                    }
                };

                buildingcontext.Reports.Add(report);

                buildingcontext.SaveChanges();
            }
        }

        [Test]
        public void Update_NotFound_EstimationDoNotExists()
        {
            const int estId = 13;
            _estimationRepository.Exists(estId).Returns(false);

            var respone = _estimationsController.Update(12, estId, new Estimation
            {
                Amount = 1,
                CompanyId = 1,
                Description = "desc",
                EstimationId = "EstID",
                Id = 1,
                Quantity = 1,
                SpecNumber = "SPEC",
                Unit = "unit",
                UnitPrice = 1
            });

            Assert.That(respone, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public void Update_Forbidden_UserHaveNoAccess()
        {
            const int estId = 13;
            _estimationRepository.Exists(estId).Returns(true);

            _estimationAccessRules.CanEdit(Arg.Any<Domain.Models.Estimation>(), Arg.Any<UserId>(), Arg.Any<BuildingId>())
                .Returns(false);

            var respone = _estimationsController.Update(12, estId, new Estimation
            {
                Amount = 1,
                CompanyId = 1,
                Description = "desc",
                EstimationId = "EstID",
                Id = 1,
                Quantity = 1,
                SpecNumber = "SPEC",
                Unit = "unit",
                UnitPrice = 1
            });

            Assert.That(respone, Is.TypeOf<ForbidResult>());
        }

        [Test]
        public void Update_Ok_EstimationExistsAndUserHaveAccess()
        {
            const int estId = 13;
            _estimationRepository.Exists(estId).Returns(true);

            _estimationAccessRules.CanEdit(Arg.Any<Domain.Models.Estimation>(), Arg.Any<UserId>(), Arg.Any<BuildingId>())
                .Returns(true);

            var respone = _estimationsController.Update(12, estId, new Estimation
            {
                Amount = 1,
                CompanyId = 1,
                Description = "desc",
                EstimationId = "EstID",
                Id = 1,
                Quantity = 1,
                SpecNumber = "SPEC",
                Unit = "unit",
                UnitPrice = 1
            });

            Assert.That(respone, Is.TypeOf<OkResult>());
        }

        [Test]
        public void GetEstimationReport_NotFound_EstimationNotExists()
        {
            const int estId = 13;
            _estimationRepository.Exists(estId).Returns(false);

            var respone = _estimationsController.GetEstimationReport(12, estId);

            Assert.That(respone, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public void GetEstimationReport_Ok_EstimationExists()
        {
            const int estId = 13;
            _estimationRepository.Exists(estId).Returns(true);

            var respone = _estimationsController.GetEstimationReport(12, estId);

            Assert.That(respone, Is.TypeOf<OkObjectResult>());
        }
    }
}