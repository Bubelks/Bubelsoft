using BubelSoft.Core.Infrastructure;
using BubelSoft.Core.Infrastructure.Database.Repositories;
using BubelSoft.Core.Infrastructure.Database.Repositories.Interfaces;
using BubelSoft.Core.Infrastructure.Email;
using BubelSoft.Core.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi
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