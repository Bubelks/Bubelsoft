using System.Runtime.InteropServices.ComTypes;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Database.Repositories;
using WebApi.Database.Repositories.Interfaces;
using WebApi.Infrastructure.Email;
using WebApi.Services;

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

        public static void AddEmailService(this IServiceCollection services)
        {
            services.AddSingleton<IEmailMessageProvider, EmailMessageProvider>();
            services.AddScoped<IMailService, MailService>();
        }
    }
}