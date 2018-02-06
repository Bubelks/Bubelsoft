using System.Collections.Generic;
using BuildingContext;
using Microsoft.AspNetCore.Mvc;
using WebApi.Database.Repositories.Interfaces;
using WebApi.Domain.Models;

namespace WebApi.Controllers.BuildingContext
{
    public class BuildingContextController: Controller
    {
        private readonly IBuildingRepository _buildingRepository;

        public BuildingContextController(IBuildingRepository buildingRepository)
        {
            _buildingRepository = buildingRepository;
        }

        protected RepositoryFactory GetBulidingRepositoryFactory(BuildingId buildingId)
        {
            var buildingConnectionString = _buildingRepository.GetConnectionString(buildingId);
            return new RepositoryFactory(buildingConnectionString);
        }
    }
}
