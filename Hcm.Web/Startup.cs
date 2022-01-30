using Hcm.Api.Client;
using Hcm.Web.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hcm.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHcmApiClient(Configuration);
            services.AddControllersWithViews(config =>
            {
                config.Filters.Add(new AuthorizeFilter());
            })
            .AddRazorRuntimeCompilation();

            services.AddScoped<AuthenticationCookieMiddleware>();

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

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseMiddleware<AuthenticationCookieMiddleware>();
            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapControllerRoute(
                     name: "default",
                     pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
