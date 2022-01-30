using Hcm.Api.Middleware.Helpers;
using Hcm.Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Text;

namespace Hcm.Api.Configuration
{
    public static class ApplicationConfiguration
    {
        public static IServiceCollection AddServices(
            this IServiceCollection services)
        {
            services.AddScoped<UserService>();
            services.AddScoped<EmployeeService>();
            services.AddScoped<AssignmentService>();
            services.AddScoped<DepartmentService>();

            services.AddScoped<ErrorResponseHandler>();
            services.AddSingleton<PasswordService>();
            services.AddScoped<TokenService>();

            return services;
        }

        public static IServiceCollection AddMappings(
            this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetCallingAssembly());
            return services;
        }

        public static IServiceCollection AddSecurity(
            this IServiceCollection services)
        {

            services.AddAuthentication(configure =>
            {
                configure.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                configure.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(configure =>
            {
                configure.Audience = "hcm";
                configure.ClaimsIssuer = "hcm.api";
                configure.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = "hcm.api",
                    ValidAudience = "hcm",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SUPER SECRET KEY 1234")),

                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    RequireSignedTokens = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

            services.AddAuthorization(configure =>
            {
                configure.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build();
            });


            return services;
        }
    }
}
