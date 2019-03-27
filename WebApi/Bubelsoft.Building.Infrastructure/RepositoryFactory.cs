using BubelSoft.Building.Infrastructure.Repositories;

namespace BubelSoft.Building.Infrastructure
{
    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly BuildingContextFactory _buildingContext;

        public RepositoryFactory()
        {
            _buildingContext = new BuildingContextFactory();
        }

        public IEstimationRepository Estimation(string connectionString) => new EstimationRepository(_buildingContext.CreateBuildingContext(connectionString));
        public IReportRepository Report(string connectionString) => new ReportRepository(_buildingContext.CreateBuildingContext(connectionString));
    }
}
