﻿using ERP.Server.Domain.Entities;
using ERP.Server.Infrastructure.Context;
using ERP.Server.Infrastructure.Options;
using GenericRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Scrutor;
using System.Reflection;

namespace ERP.Server.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("SqlServer");
        Console.WriteLine($"Connection String: {connectionString}");  // Debugging log ekleyin

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        services.AddScoped<IUnitOfWork>(srv => srv.GetRequiredService<ApplicationDbContext>());

        services
            .AddIdentity<AppUser, IdentityRole<Guid>>(cfr =>
            {
                cfr.Password.RequiredLength = 1;
                cfr.Password.RequireNonAlphanumeric = false;
                cfr.Password.RequireUppercase = false;
                cfr.Password.RequireLowercase = false;
                cfr.Password.RequireDigit = false;
                cfr.SignIn.RequireConfirmedEmail = true;
                cfr.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                cfr.Lockout.MaxFailedAccessAttempts = 3;
                cfr.Lockout.AllowedForNewUsers = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
        services.ConfigureOptions<JwtTokenOptionsSetup>();
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer();
        services.AddAuthorization();

        services.Scan(action =>
        {
            action
            .FromAssemblies(Assembly.GetExecutingAssembly())
            .AddClasses(publicOnly: false)
            .UsingRegistrationStrategy(RegistrationStrategy.Skip)
            .AsMatchingInterface()
            .AsImplementedInterfaces()
            .WithScopedLifetime();
        });


        services.AddHealthChecks()
        .AddCheck("health-check", () => HealthCheckResult.Healthy())
        .AddDbContextCheck<ApplicationDbContext>()
        ;

        return services;
    }
}
