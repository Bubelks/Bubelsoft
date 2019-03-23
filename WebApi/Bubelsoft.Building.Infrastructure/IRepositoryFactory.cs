using Bubelsoft.Building.Infrastructure.Repositories;

namespace Bubelsoft.Building.Infrastructure
{
    public interface IRepositoryFactory
    {
        IEstimationRepository Estimation(string connectionString);
        IReportRepository Report(string connectionString);
    }
}