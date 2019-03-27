using BubelSoft.Building.Domain.AccessRules;
using Microsoft.Extensions.DependencyInjection;

namespace BubelSoft.Building.Infrastructure
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