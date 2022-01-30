using Hcm.Core.Database;
using Hcm.Database.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Hcm.Database
{
    public static class ApplicationConfiguration
    {
        public static IServiceCollection AddDatabase(
            this IServiceCollection services, 
            Assembly migrationAssembly)
        {
            services.AddScoped<IDatabaseContext>(context =>
            {
                return context.GetRequiredService<DatabaseContext>();
            });

            services.AddDbContext<DatabaseContext>(configure =>
            {
                configure.UseSqlite("Data Source=dss.db", opt =>
                {
                    opt.MigrationsAssembly(migrationAssembly.GetName().Name);
                    opt.MigrationsHistoryTable("migration", "dbo");
                });

                configure.EnableDetailedErrors(true);
                configure.ConfigureWarnings(opt =>
                {
                    opt.Default(WarningBehavior.Log);
                });
            });
            
            return services;
        }

        public static IServiceCollection AddRepositories(
            this IServiceCollection services)
        {
            // Add Unit of work implementation
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Add Repositories
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ISallaryRepository, SallaryRepository>();
            services.AddScoped<IAssignmentRepository, AssignmentRepository>();

            return services;
        }

        public static IApplicationBuilder UseDatabase(this IApplicationBuilder app)
        {
            var context = app.ApplicationServices.CreateScope()
                .ServiceProvider
                .GetRequiredService<DatabaseContext>();

            context.Database.Migrate();

            return app;
        }
    }
}
