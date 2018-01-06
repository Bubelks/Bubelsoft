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
            var buildings = new[]
            {
                new BuildingDto("Building1", true, new DTO.Company
                {
                    Id = 1,
                    Name = "Company1"
                }),
                new BuildingDto("building2", false, new DTO.Company
                {
                    Id = 2,
                    Name = "Company 2"
                })
            };

            return Ok(buildings);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var building = _buildingRepository.Get(new BuildingId(id));

            if (building == null)
                return NotFound();

            return Ok(new BuildingDto(building.Name, true, new DTO.Company
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

        // POST api/values
        [HttpPost]
        public void Create([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]DTO.BuildingCreation buildingCreation)
        {
            var company = new Company(
                new CompanyId(buildingCreation.Company.Id),
                buildingCreation.Company.Name,
                buildingCreation.Company.Nip,
                buildingCreation.Company.PhoneNumber,
                buildingCreation.Company.Email,
                buildingCreation.Company.City,
                buildingCreation.Company.PostCode,
                buildingCreation.Company.Street,
                buildingCreation.Company.PlaceNumber
                );
            var building = new Building(new BuildingId(id), buildingCreation.Name, company);
            _buildingRepository.Save(building);
            _companyRepository.Save(company);
            return Ok();

        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }

    internal class BuildingDto
    {
        public BuildingDto(string name, bool ownedByMy, DTO.Company company)
        {
            Name = name;
            OwnedByMy = ownedByMy;
            Company = company;
        }

        public string Name { get; }

        public bool OwnedByMy { get; }

        public DTO.Company Company { get; }
    }
}
