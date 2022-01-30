using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hcm.Api.Client
{
    public static class ApplicationConfiguration
    {
        public static IServiceCollection AddHcmApiClient(
            this IServiceCollection services, 
            IConfiguration configuration)
        {
            services.Configure<HcmApiClientSettings>(configuration.GetSection(nameof(HcmApiClientSettings)));
            services.AddScoped<HcmApiHttpClient>();

            services.AddScoped<IAssignmentClient, AssignmentClient>();
            services.AddScoped<IEmployeeClient, EmployeeClient>();
            services.AddScoped<IDepartmentClient, DepartmentClient>();
            services.AddScoped<IUserClient, UserClient>();
            services.AddScoped<ITokenClient, TokenClient>();
            services.AddHttpContextAccessor();

            return services;
        }
    }
}
