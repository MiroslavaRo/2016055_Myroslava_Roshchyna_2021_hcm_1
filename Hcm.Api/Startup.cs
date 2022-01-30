using Hcm.Api.Configuration;
using Hcm.Api.Middleware;
using Hcm.Database;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Reflection;

namespace Hcm
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
            services.AddDatabase(Assembly.GetAssembly(typeof(Startup)));
            services.AddRepositories();
            services.AddControllers(mvc => mvc.Filters.Add(new AuthorizeFilter()));
            services.AddServices();
            services.AddMappings();
            services.AddSecurity();
            services.AddLogging(configure =>
            {
                configure.ClearProviders();
                configure.AddConsole(setup =>
                {
                    setup.DisableColors = false;
                    setup.IncludeScopes = true;
                });
                configure.SetMinimumLevel(LogLevel.Information);
            });
            services.AddScoped<TransactionMiddleware>();
            services.AddScoped<ExceptionMiddleware>();

            services.AddSwaggerGen(setup =>
            {
                setup.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo 
                { 
                    Title = "Hcm.Api", 
                    Version = "v1" 
                });

                setup.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                    Description = "JWT Authorization header using the Bearer scheme. Example 'Auhtorizaion: Bearer {token}'"
                });

                setup.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                        {
                            Reference = new Microsoft.OpenApi.Models.OpenApiReference {
                                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme
                            }
                        }, new List<string>()
                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<TransactionMiddleware>();
            app.UseMiddleware<ExceptionMiddleware>();
           
            app.UseSwagger();
            app.UseSwaggerUI(setup =>
            {
                setup.RoutePrefix = string.Empty;

                setup.SwaggerEndpoint(
                    url: "/swagger/v1/swagger.json",
                    name: "Hcm.Api");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseDatabase();
        }
    }
}
