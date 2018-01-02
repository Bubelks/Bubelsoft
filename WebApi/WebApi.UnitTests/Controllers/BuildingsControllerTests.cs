﻿using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using WebApi.Controllers;
using WebApi.Controllers.Interfaces;
using WebApi.Database.Repositories.Interfaces;
using WebApi.Domain.Models;

namespace WebApi.UnitTests.Controllers
{
    [TestFixture]
    public class BuildingsControllerTests
    {
        private IBuildingsController _controller;
        private IBuildingRepository _repository;
        private ICompanyRepository _companyRepository;

        [SetUp]
        public void Init()
        {
            _repository = Substitute.For<IBuildingRepository>();
            _companyRepository = Substitute.For<ICompanyRepository>();
            _controller = new BuildingsController(_repository, _companyRepository);
        }

        [Test]
        public void Get_OkWithNothing_ThereIsNoBuildings()
        {
            var userId = new UserId(1);
            _repository.GetFor(userId).Returns(new Building[] { });

            var result = _controller.Get();

            Assert.That(result, Is.TypeOf<OkObjectResult>());
        }
    }
}