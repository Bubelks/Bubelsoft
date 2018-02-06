using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Database.Repositories.Interfaces;
using WebApi.Domain.Models;
using WebApi.Infrastructure;

namespace WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class BuildingsController : Controller
    {
        private readonly IBuildingRepository _buildingRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly ICurrentUser _currentUser;

        public BuildingsController(IBuildingRepository buildingRepository, ICompanyRepository companyRepository, ICurrentUser currentUser)
        {
            _buildingRepository = buildingRepository;
            _companyRepository = companyRepository;
            _currentUser = currentUser;
        }

        // GET api/buildings
        [HttpGet]
        public IActionResult Get()
        {
            var userBuildings = _buildingRepository.GetFor(_currentUser.Id);

            var buildings = userBuildings.Select(b => new BuildingsListItem{
                Id = b.Id.Value,
                Name = b.Name,
                OwnedByMy = b.IsOwnedBy(_currentUser.User),
                CompanyName = b.MainContractor.Name,
                CompanyId = b.MainContractor.Id.Value,
                UserBuildingRoles = _currentUser.User.Roles.Where(r => r.BuildingId == b.Id).Select(r => r.UserBuildingRole)
            });

            return Ok(buildings);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var building = _buildingRepository.Get(new BuildingId(id));

            if (building == null)
                return NotFound();

            return Ok(new BuildingDto(building.Name, building.IsOwnedBy(_currentUser.User), new DTO.CompanyInfo
            {
                Id = building.MainContractor.Id.Value,
                Name = building.MainContractor.Name,
                Nip = building.MainContractor.Nip,
                PhoneNumber = building.MainContractor.PhoneNumber,
                Email = building.MainContractor.Email,
                City = building.MainContractor.City,
                PostCode = building.MainContractor.PostCode,
                Street = building.MainContractor.Street,
                PlaceNumber = building.MainContractor.PlaceNumber,
            }));
        }
    }

    internal class BuildingDto
    {
        public BuildingDto(string name, bool ownedByMy, DTO.CompanyBase company)
        {
            Name = name;
            OwnedByMy = ownedByMy;
            Company = company;
        }

        public string Name { get; }

        public bool OwnedByMy { get; }

        public DTO.CompanyBase Company { get; }
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
