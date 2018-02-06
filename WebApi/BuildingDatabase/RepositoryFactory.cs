using BuildingContext.Repositories;

namespace BuildingContext
{
    public class RepositoryFactory
    {
        private readonly BuildingContext _buildingContext;

        public RepositoryFactory(string connectionString)
        {
            _buildingContext = new BuildingContextFactory().CreateBuildingContext(connectionString);
        }

        public IEstimationRepository Estimation => new EstimationRepository(_buildingContext);
        public IReportRepository Report => new ReportRepository(_buildingContext);
    }
}
