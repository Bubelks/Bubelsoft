
using BubelSoft.Core.Domain.Models;
using BubelSoft.Security;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi
{
    public static class ServiceCollectionExtension
    {
        public static void AddBubelSoftSecurity(this IServiceCollection services)
        {
            services.AddScoped<IUserSession, UserSession>();
            services.AddSingleton<IBubelSoftJwtToken, BubelSoftJwtToken>();
            services.AddSingleton<IBubelSoftUserPassword, BubelSoftUserPassword>();
        }
    }
}