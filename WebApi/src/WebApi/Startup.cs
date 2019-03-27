using System.Text;
using System.Threading.Tasks;
using BubelSoft.Building.Infrastructure;
using BubelSoft.Core.Infrastructure;
using BubelSoft.Core.Infrastructure.Database;
using BubelSoft.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

[assembly: ApiController]
namespace WebApi
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsEnvironment("Development"))
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                /*builder.AddApplicationInsightsSettings(developerMode: true);*/
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            //// Add framework services.
            //services.AddApplicationInsightsTelemetry(Configuration);
            services.AddSingleton(Configuration);
            services.AddCors();
            services.AddAuthentication(ao =>
                {
                    ao.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    ao.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(o =>
                {
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = Configuration["Tokens:Issuer"],
                        ValidateAudience = true,
                        ValidAudience = Configuration["Tokens:Audience"],
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Tokens:Key"])),
                        ValidateLifetime = true,
                        RequireExpirationTime = true,
                        RequireSignedTokens = true
                    };
                    o.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = context =>
                        {
                            UserSession.UpdateUser(context.Principal, context.HttpContext);
                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddDbContext<MainContext>(options => options.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"]));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddBubelSoftCore();
            services.AddBubelSoftBuilding();
            services.AddBubelSoftSecurity();

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors(builder =>
                builder.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod());

            app.UseAuthentication();
            //app.UseApplicationInsightsRequestTelemetry();
            //app.UseApplicationInsightsExceptionTelemetry();

            app.UseMvc();
        }
    }
}
