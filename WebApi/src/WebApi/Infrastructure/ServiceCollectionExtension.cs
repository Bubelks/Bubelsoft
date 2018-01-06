using Microsoft.Extensions.DependencyInjection;
using WebApi.Database.Repositories;
using WebApi.Database.Repositories.Interfaces;

namespace WebApi.Infrastructure
{
    public static class ServiceCollectionExtension
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IBuildingRepository, BuildingRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
        }

        public static void AddCurrentUser(this IServiceCollection services)
        {
            services.AddScoped<ICurrentUser, CurrentUser>();
        }
    }
}