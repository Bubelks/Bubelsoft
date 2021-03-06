﻿using System.Collections.Generic;
using System.Linq;
using BubelSoft.Core.Domain.Models;
using BubelSoft.Core.Infrastructure.Controllers.DTO;
using BubelSoft.Core.Infrastructure.Database.Repositories.Interfaces;
using BubelSoft.Core.Infrastructure.Services;
using BubelSoft.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Company = BubelSoft.Core.Domain.Models.Company;

namespace BubelSoft.Core.Infrastructure.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class BuildingsController : Controller
    {
        private readonly IBuildingRepository _buildingRepository;
        private readonly IUserSession _userSession;
        private readonly ICompanyRepository _companyRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMailService _mailService;

        public BuildingsController(
            IBuildingRepository buildingRepository,
            ICompanyRepository companyRepository,
            IUserSession userSession,
            IUserRepository userRepository,
            IMailService mailService)
        {
            _buildingRepository = buildingRepository;
            _userSession = userSession;
            _companyRepository = companyRepository;
            _userRepository = userRepository;
            _mailService = mailService;
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]BuildingCreation buildingCreation)
        {
            var company = new Company(
                new CompanyId(buildingCreation.Company.Id),
                buildingCreation.Company.Name,
                buildingCreation.Company.Nip
            );

            var building = new Building(new BuildingId(id), buildingCreation.Name, company, new List<Company>());
            _buildingRepository.Save(building);
            _companyRepository.Save(company);
            return Ok();
        }
        
        [HttpGet]
        public IActionResult Get()
        {
            var userBuildings = _buildingRepository.GetFor(_userSession.User.Id);

            var buildings = userBuildings.Select(b => new BuildingsListItem{
                Id = b.Id.Value,
                Name = b.Name,
                OwnedByMy = b.IsOwnedBy(_userSession.User),
                CompanyName = b.MainContractor.Name,
                CompanyId = b.MainContractor.Id.Value,
                UserBuildingRoles = _userSession.User.Roles.Where(r => r.BuildingId == b.Id).Select(r => r.UserBuildingRole)
            });

            return Ok(buildings);
        }
        
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var buildingId = new BuildingId(id);
            var building = _buildingRepository.Get(buildingId);

            if (building == null)
                return NotFound();

            if (!building.CanAccess(_userSession.User))
                return Forbid();

            return Ok(new BuildingDto(building.Name, building.IsOwnedBy(_userSession.User), new CompanyInfo
            {
                Id = building.MainContractor.Id.Value,
                Name = building.MainContractor.Name,
                Nip = building.MainContractor.Number
            }));
        }

        [HttpPost("{id}/worker/add")]
        public IActionResult AddWorker(int id, [FromBody]BuildingWorker worker)
        {
            var buildingId = new BuildingId(id);
            var user = _userRepository.Get(new UserId(worker.UserId));

            foreach (var workerUserBuildingRole in worker.UserBuildingRoles)
            {
                user.AddRole(buildingId, workerUserBuildingRole);
            }

            _userRepository.Save(user);

            return Ok();
        }

        [HttpGet("{id}/companies")]
        public IActionResult GetCompanies(int id)
        {
            var buildingId = new BuildingId(id);
            var building = _buildingRepository.Get(buildingId);

            if (building == null)
                return NotFound();

            if (!building.CanAccess(_userSession.User))
                return Forbid();

            var companies = building.SubContractors.Union(new[] { building.MainContractor });
            return Ok(companies.Select(c
                => new BuildingCompany
                {
                    Id = c.Id.Value,
                    Name = c.Name,
                    MainContract = building.MainContractor.Id == c.Id,
                    CanAddWorker = c.Id == _userSession.User.CompanyId,
                    Workers = _userRepository.GetBuildingWorkers(buildingId, c.Id)
                        .Select(u => new BuildingWorker
                        {
                            UserId = u.Id.Value,
                            DisplayName = $"{u.FirstName} {u.LastName} ({u.Email})",
                            UserBuildingRoles = u.Roles.Where(r => r.BuildingId == buildingId).Select(r => r.UserBuildingRole)
                        })
                }));
        }

        [HttpPut("{id}/companies/add")]
        public IActionResult AddNewSub(int id, [FromBody]string email)
        {
            var building = _buildingRepository.Get(new BuildingId(id));

            _mailService.SendCompanyInvitedInfo(_userSession.User, building, email);

            return Ok();
        }

        [HttpPost("{id}/companies/add")]
        public IActionResult AddSub(int id, [FromBody]int companyId)
        {
            _companyRepository.AddSubContract(new BuildingId(id), new CompanyId(companyId));
            return Ok();
        }
    }
    
    public class BuildingCompany
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool MainContract { get; set; }

        public IEnumerable<BuildingWorker> Workers { get; set; }
        public bool CanAddWorker { get; set; }
    }

    public class BuildingWorker
    {
        public int UserId { get; set; }
        public string DisplayName { get; set; }
        public IEnumerable<UserBuildingRole> UserBuildingRoles { get; set; }
    }

    internal class BuildingDto
    {
        public BuildingDto(string name, bool ownedByMy, CompanyBase company)
        {
            Name = name;
            OwnedByMy = ownedByMy;
            Company = company;
        }

        public string Name { get; }

        public bool OwnedByMy { get; }

        public CompanyBase Company { get; }
    }

    internal class BuildingsListItem
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool OwnedByMy { get; set; }

        public string CompanyName { get; set; }

        public int CompanyId { get; set; }

        public IEnumerable<UserBuildingRole> UserBuildingRoles { get; set; }
    }
}
