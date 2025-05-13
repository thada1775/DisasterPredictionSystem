using DisasterPrediction.Application.Common.Interfaces;
using DisasterPrediction.Application.Common.Interfaces.Auth;
using DisasterPrediction.Application.Interfaces;
using DisasterPrediction.Application.Services;
using DisasterPrediction.Domain.Entities;
using DisasterPrediction.Infrastructure.Common;
using DisasterPrediction.Infrastructure.Data;
using DisasterPrediction.Infrastructure.Services;
using DisasterPrediction.Infrastructure.Services.Auth;
using DisasterPrediction.Infrastructure.Services.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisasterPrediction.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            //var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<ICurrentUserService, CurrentUserService>();

            Console.WriteLine(configuration.GetConnectionString("DefaultConnection"));

            services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            // Redis Cache
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetSection("Redis")["ConnectionString"];
            });

            services.AddScoped<ICacheService, RedisCacheService>();

            services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
            services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IRegionService, RegionService>();
            services.AddScoped<IAlertSettingService, AlertSettingService>();
            services.AddScoped<IAlertsService, AlertsService>();
            services.AddScoped<IDisasterService, DisasterService>();

            services.AddHttpClient<IApiService, ApiService>();
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            services.AddTransient<IEmailService, EmailService>();

            return services;
        }
    }
}
