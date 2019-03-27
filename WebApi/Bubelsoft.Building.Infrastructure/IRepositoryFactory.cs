using BubelSoft.Building.Infrastructure.Repositories;

namespace BubelSoft.Building.Infrastructure
{
    public interface IRepositoryFactory
    {
        IEstimationRepository Estimation(string connectionString);
        IReportRepository Report(string connectionString);
    }
}