using BubelSoft.Building.Infrastructure.Repositories;
using BubelSoft.Core.Domain.Models;
using BubelSoft.Core.Infrastructure.Database.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BubelSoft.Building.Infrastructure.Controllers
{
    public class BuildingContextController: Controller
    {
        protected readonly IBuildingRepository BuildingRepository;
        private readonly IRepositoryFactory _repositoryFactory;

        public BuildingContextController(IBuildingRepository buildingRepository, IRepositoryFactory repositoryFactory)
        {
            BuildingRepository = buildingRepository;
            _repositoryFactory = repositoryFactory;
        }

        protected IReportRepository ReportRepository(BuildingId buildingId)
        {
            var buildingConnectionString = BuildingRepository.GetConnectionString(buildingId);
            return _repositoryFactory.Report(buildingConnectionString);

        }
        protected IEstimationRepository EstimationRepository(BuildingId buildingId)
        {
            var buildingConnectionString = BuildingRepository.GetConnectionString(buildingId);
            return _repositoryFactory.Estimation(buildingConnectionString);

        }
    }
}
