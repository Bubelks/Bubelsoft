using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Controllers.Interfaces;
using WebApi.Database.Repositories.Interfaces;
using WebApi.Domain.Models;

namespace WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class BuildingsController : Controller, IBuildingsController
    {
        private readonly IBuildingRepository _buildingRepository;

        public BuildingsController(IBuildingRepository buildingRepository)
        {
            _buildingRepository = buildingRepository;
        }

        // GET api/buildings
        [HttpGet]
        public IActionResult Get()
        {
            var buildings = new[]
            {
                new BuildingDto("Building1", true),
                new BuildingDto("building2", false)
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

            return Ok();
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }

    internal class BuildingDto
    {
        public BuildingDto(string name, bool ownedByMy)
        {
            Name = name;
            OwnedByMy = ownedByMy;
        }

        public string Name { get; }

        public bool OwnedByMy { get; }
    }
}
