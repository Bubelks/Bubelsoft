using BubelSoft.Core.Infrastructure.Database.Repositories;
using BubelSoft.Core.Infrastructure.Database.Repositories.Interfaces;
using BubelSoft.Core.Infrastructure.Email;
using BubelSoft.Core.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BubelSoft.Core.Infrastructure
{
    public static class ServiceCollectionExtension
    {
        public static void AddBubelSoftCore(this IServiceCollection services)
        {
            services.AddScoped<IBuildingRepository, BuildingRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<ICurrentUser, CurrentUser>();
            services.AddSingleton<IEmailMessageProvider, EmailMessageProvider>();
            services.AddScoped<IMailService, MailService>();
        }
    }
}