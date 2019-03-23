using BubelSoft.Building.Domain.AccessRules;
using Microsoft.Extensions.DependencyInjection;

namespace Bubelsoft.Building.Infrastructure
{
    public static class ServiceCollectionExtension
    {
        public static void AddBubelSoftBuilding(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryFactory, RepositoryFactory>();
            services.AddScoped<IReportAccessRules, ReportAccessRules>();
            services.AddScoped<IEstimationAccessRules, EstimationAccessRules>();
        }
    }
}